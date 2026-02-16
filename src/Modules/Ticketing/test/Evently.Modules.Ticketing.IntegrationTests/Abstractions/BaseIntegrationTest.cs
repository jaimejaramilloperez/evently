using Bogus;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Customers.CreateCustomer;
using Evently.Modules.Ticketing.Application.Events.CreateEvent;
using Evently.Modules.Ticketing.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Ticketing.IntegrationTests.Abstractions;

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

    public async Task CleanDatabaseAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = appFactory.Services.CreateScope();
        TicketingDbContext dbContext = scope.ServiceProvider.GetRequiredService<TicketingDbContext>();

        await dbContext.Database.ExecuteSqlRawAsync(
            """
            DELETE FROM ticketing.inbox_message_consumers;
            DELETE FROM ticketing.inbox_messages;
            DELETE FROM ticketing.outbox_message_consumers;
            DELETE FROM ticketing.outbox_messages;
            DELETE FROM ticketing.events;
            DELETE FROM ticketing.ticket_types;
            DELETE FROM ticketing.customers;
            DELETE FROM ticketing.orders;
            DELETE FROM ticketing.order_items;
            DELETE FROM ticketing.tickets;
            DELETE FROM ticketing.payments;
            """, cancellationToken);
    }

    public async Task<Guid> CreateCustomerAsync(Guid customerId, CancellationToken cancellationToken)
    {
        Result result = await SendAsync(new CreateCustomerCommand()
        {
            CustomerId = customerId,
            Email = Faker.Internet.Email(),
            FirstName = Faker.Person.FirstName,
            LastName = Faker.Person.LastName,
        }, cancellationToken);

        Assert.True(result.IsSuccess);

        return customerId;
    }

    public async Task CreateEventWithTicketTypeAsync(
        Guid eventId,
        Guid ticketTypeId,
        decimal quantity,
        CancellationToken cancellationToken)
    {
        CreateEventCommand.TicketTypeRequest ticketType = new()
        {
            TicketTypeId = ticketTypeId,
            EventId = eventId,
            Name = Faker.Random.AlphaNumeric(10),
            Price = Faker.Random.Decimal(),
            Currency = Faker.Random.AlphaNumeric(3),
            Quantity = quantity,
        };

        Result result = await SendAsync(new CreateEventCommand()
        {
            EventId = eventId,
            Title = Faker.Random.AlphaNumeric(10),
            Description = Faker.Random.AlphaNumeric(10),
            Location = Faker.Address.FullAddress(),
            StartsAtUtc = DateTime.UtcNow,
            EndsAtUtc = null,
            TicketTypes = [ticketType],
        }, cancellationToken);

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
