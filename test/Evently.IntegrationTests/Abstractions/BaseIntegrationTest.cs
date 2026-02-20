using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Bogus;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Events.CreateEvent;
using Evently.Modules.Users.Application.Users.RegisterUser;
using Evently.Modules.Users.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Evently.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest : IDisposable
{
    private readonly ApiIntegrationTestWebAppFactory _apiFactory;

    protected static readonly Faker Faker = new();

    protected HttpClient ApiClient { get; init; }
    protected HttpClient TicketingApiClient { get; init; }

    protected BaseIntegrationTest(
        ApiIntegrationTestWebAppFactory apiFactory,
        TicketingApiIntegrationTestWebAppFactory ticketingApiFactory)
    {
        _apiFactory = apiFactory;

        ApiClient = apiFactory.CreateClient();
        TicketingApiClient = ticketingApiFactory.CreateClient();
    }

    public async Task<string> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        using HttpClient authClient = new();

        using IServiceScope scope = _apiFactory.Services.CreateScope();
        KeyCloakOptions options = scope.ServiceProvider.GetRequiredService<IOptions<KeyCloakOptions>>().Value;

        Dictionary<string, string> authRequestParameters = new()
        {
            ["client_id"] = options.PublicClientId,
            ["grant_type"] = "password",
            ["scope"] = "openid email",
            ["username"] = email,
            ["password"] = password,
        };

        using FormUrlEncodedContent authRequestContent = new(authRequestParameters);

        using HttpRequestMessage authRequest = new(HttpMethod.Post, new Uri(options.TokenUrl, UriKind.Absolute));
        authRequest.Content = authRequestContent;

        using HttpResponseMessage authorizationResponse = await authClient.SendAsync(authRequest, cancellationToken);
        authorizationResponse.EnsureSuccessStatusCode();

        AuthToken? token = await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>(cancellationToken);

        return token?.AccessToken ?? throw new InvalidOperationException("Failed to deserialize authorization token from Keycloak");
    }

    protected static void AuthenticateClient(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new(JwtBearerDefaults.AuthenticationScheme, token);
    }

    public async Task<Result<Guid>> RegisterUserAsync(string email, string password, CancellationToken cancellationToken)
    {
        RegisterUserCommand request = new()
        {
            Email = email,
            Password = password,
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        HttpResponseMessage response = await ApiClient.PostAsJsonAsync(
            "api/users/register",
            request,
            cancellationToken);

        string? id = await response.Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out Guid parsedId))
        {
            return Result.Failure<Guid>(Error.Failure("DeserializationError", "Failed to deserialize response"));
        }

        return parsedId;
    }

    public async Task CreateEventAsync(
        Guid eventId,
        Guid ticketTypeId,
        decimal quantity,
        string accessToken,
        CancellationToken cancellationToken = default)
    {
        using IServiceScope scope = _apiFactory.Services.CreateScope();
        ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();

        CreateEventCommand.TicketTypeRequest ticketType = new()
        {
            TicketTypeId = ticketTypeId,
            EventId = eventId,
            Name = Faker.Random.AlphaNumeric(10),
            Price = Faker.Random.Decimal(),
            Currency = Faker.Random.AlphaNumeric(3),
            Quantity = quantity,
        };

        CreateEventCommand command = new()
        {
            EventId = eventId,
            Title = Faker.Random.AlphaNumeric(10),
            Description = Faker.Random.AlphaNumeric(10),
            Location = Faker.Address.FullAddress(),
            StartsAtUtc = DateTime.UtcNow,
            EndsAtUtc = null,
            TicketTypes = [ticketType],
        };

        Result result = await sender.Send(command, cancellationToken);
        Assert.True(result.IsSuccess);
    }

    internal sealed class AuthToken
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; init; }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        ApiClient.Dispose();
        TicketingApiClient.Dispose();
    }
}
