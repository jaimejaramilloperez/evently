using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Bogus;
using Evently.Modules.Users.Infrastructure.Database;
using Evently.Modules.Users.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Evently.Modules.Users.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public class BaseIntegrationTest(IntegrationTestWebAppFactory appFactory) : IDisposable
{
    protected static readonly Faker Faker = new();

    private HttpClient? _authenticatedClient;

    public HttpClient CreateClient() => appFactory.CreateClient();

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        using IServiceScope scope = appFactory.Services.CreateScope();
        ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();

        return await sender.Send(request, cancellationToken);
    }

    public async Task<HttpClient> CreateAuthenticatedClientAsync(
        string email = $"test-user@example.com",
        string password = "StrongPass12345!",
        CancellationToken cancellationToken = default)
    {
        if (_authenticatedClient is not null)
        {
            return _authenticatedClient;
        }

        HttpClient client = CreateClient();

        bool userExists = false;

        using (IServiceScope serviceScope = appFactory.Services.CreateScope())
        {
            using UsersDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<UsersDbContext>();
            userExists = await dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }

        if (!userExists)
        {
            HttpResponseMessage registerResponse = await client.PostAsJsonAsync(
                "/api/users/register",
                new Presentation.Users.RegisterUser.Request
                {
                    Email = email,
                    Password = password,
                    FirstName = Faker.Name.FirstName(),
                    LastName = Faker.Name.LastName(),
                },
                cancellationToken);

            registerResponse.EnsureSuccessStatusCode();
        }

        string accessToken = await GetAccessTokenAsync(email, password, cancellationToken);

        client.DefaultRequestHeaders.Authorization = new(JwtBearerDefaults.AuthenticationScheme, accessToken);
        _authenticatedClient = client;

        return _authenticatedClient;
    }

    public async Task<string> GetAccessTokenAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        using HttpClient httpClient = new();

        using IServiceScope scope = appFactory.Services.CreateScope();
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

        using HttpResponseMessage authorizationResponse = await httpClient.SendAsync(authRequest, cancellationToken);
        authorizationResponse.EnsureSuccessStatusCode();

        AuthToken? token = await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>(cancellationToken);

        return token?.AccessToken ?? throw new InvalidOperationException("Failed to deserialize authorization token from Keycloak");
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
        _authenticatedClient?.Dispose();
    }
}
