using Bogus;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Categories.CreateCategory;
using Evently.Modules.Events.Application.Categories.GetCategory;
using Evently.Modules.Events.Application.Events.CreateEvent;
using Evently.Modules.Events.Application.Events.GetEvents;
using Evently.Modules.Events.Application.TicketTypes.CreateTicketType;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

namespace Evently.Modules.Events.IntegrationTests.Abstractions;

[Collection(nameof(IntegrationTestCollection))]
public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory appFactory) : IDisposable
{
    protected static readonly Faker Faker = new();
    protected FakeTimeProvider FakeTimeProvider => appFactory.FakeTimeProvider;

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        using IServiceScope scope = appFactory.Services.CreateScope();
        ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();

        return await sender.Send(request, cancellationToken);
    }

    public async Task CleanDatabaseAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = appFactory.Services.CreateScope();
        EventsDbContext dbContext = scope.ServiceProvider.GetRequiredService<EventsDbContext>();

        await dbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM events.inbox_message_consumers;
            DELETE FROM events.inbox_messages;
            DELETE FROM events.outbox_message_consumers;
            DELETE FROM events.outbox_messages;
            DELETE FROM events.ticket_types;
            DELETE FROM events.events;
            DELETE FROM events.categories;
            """, cancellationToken);
    }

    public async Task<Guid> CreateCategoryAsync(string name, CancellationToken cancellationToken)
    {
        Result<CategoryResponse> result = await SendAsync(new CreateCategoryCommand(name), cancellationToken);
        return result.Value.Id;
    }

    public async Task<Guid> CreateEventAsync(
        Guid categoryId,
        DateTime? startsAtUtc = null,
        CancellationToken cancellationToken = default)
    {
        DateTime defaultStartTime = FakeTimeProvider.GetUtcNow().AddMinutes(10).UtcDateTime;

        Result<EventResponse> result = await SendAsync(new CreateEventCommand()
        {
            CategoryId = categoryId,
            Title = Faker.Random.AlphaNumeric(10),
            Description = Faker.Random.AlphaNumeric(10),
            Location = Faker.Random.AlphaNumeric(10),
            StartsAtUtc = startsAtUtc ?? defaultStartTime,
            EndsAtUtc = null,
        }, cancellationToken);

        return result.Value.Id;
    }

    public async Task<Guid> CreateTicketTypeAsync(Guid eventId, CancellationToken cancellationToken)
    {
        Result<TicketTypeResponse> result = await SendAsync(new CreateTicketTypeCommand()
        {
            EventId = eventId,
            Name = Faker.Random.AlphaNumeric(10),
            Price = Faker.Random.Decimal(),
            Currency = Faker.Random.AlphaNumeric(3),
            Quantity = Faker.Random.Decimal()
        }, cancellationToken);

        return result.Value.Id;
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
