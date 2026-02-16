using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;
using Evently.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Modules.Ticketing.UnitTests.Tickets;

public class TicketTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenTicketIsCreated()
    {
        // Arrange
        Customer customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Order order = Order.Create(customer);

        DateTime startsAtUtc = DateTime.UtcNow;

        Event @event = Event.Create(
            Guid.NewGuid(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        TicketType ticketType = TicketType.Create(
            Guid.NewGuid(),
            @event.Id,
            Faker.Name.FirstName(),
            Faker.Random.Decimal(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.Decimal());

        // Act
        Result<Ticket> result = Ticket.Create(
            order,
            ticketType);

        // Assert
        TicketCreatedDomainEvent domainEvent = AssertDomainEventWasPublished<TicketCreatedDomainEvent>(result.Value);
        Assert.Equal(result.Value.Id, domainEvent.TicketId);
    }

    [Fact]
    public void Archive_ShouldRaiseDomainEvent_WhenTicketIsArchived()
    {
        // Arrange
        Customer customer = Customer.Create(
            Guid.NewGuid(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Order order = Order.Create(customer);

        DateTime startsAtUtc = DateTime.UtcNow;

        Event @event = Event.Create(
            Guid.NewGuid(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        TicketType ticketType = TicketType.Create(
            Guid.NewGuid(),
            @event.Id,
            Faker.Name.FirstName(),
            Faker.Random.Decimal(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.Decimal());

        Result<Ticket> result = Ticket.Create(
            order,
            ticketType);

        // Act
        result.Value.Archive();

        // Assert
        TicketArchivedDomainEvent domainEvent = AssertDomainEventWasPublished<TicketArchivedDomainEvent>(result.Value);
        Assert.Equal(result.Value.Id, domainEvent.TicketId);
    }
}
