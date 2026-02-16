using Bogus;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Events.CreateEvent;
using Evently.Modules.Users.Application.Users.RegisterUser;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory appFactory) : IDisposable
{
    protected static readonly Faker Faker = new();

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        using IServiceScope scope = appFactory.Services.CreateScope();
        ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();

        return await sender.Send(request, cancellationToken);
    }

    public async Task<Result<Guid>> RegisterUserAsync(CancellationToken cancellationToken)
    {
        RegisterUserCommand command = new()
        {
            Email = Faker.Internet.Email(),
            Password = Faker.Internet.Password(6),
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        Result<Guid> userResult = await SendAsync(command, cancellationToken);
        return userResult;
    }

    public async Task CreateEventAsync(
        Guid eventId,
        Guid ticketTypeId,
        decimal quantity,
        CancellationToken cancellationToken = default)
    {
        using IServiceScope scope = appFactory.Services.CreateScope();
        ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();

        CreateEventCommand.TicketTypeRequest ticketType = new()
        {
            TicketTypeId = ticketTypeId,
            EventId = eventId,
            Name = Faker.Music.Genre(),
            Price = Faker.Random.Decimal(),
            Currency = "USD",
            Quantity = quantity,
        };

        CreateEventCommand command = new()
        {
            EventId = eventId,
            Title = Faker.Music.Genre(),
            Description = Faker.Music.Genre(),
            Location = Faker.Address.FullAddress(),
            StartsAtUtc = DateTime.UtcNow,
            EndsAtUtc = null,
            TicketTypes = [ticketType],
        };

        Result result = await sender.Send(command, cancellationToken);
        Assert.True(result.IsSuccess);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }
}
