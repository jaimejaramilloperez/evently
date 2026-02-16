using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.Domain.Tickets.DomainEvents;
using Evently.Modules.Attendance.UnitTests.Abstractions;

namespace Evently.Modules.Attendance.UnitTests.Tickets;

public class TicketTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenTicketIsCreated()
    {
        // Arrange
        Attendee attendee = Attendee.Create(
            Guid.CreateVersion7(),
            Faker.Internet.Email(),
            Faker.Person.FirstName,
            Faker.Person.LastName);

        DateTime startsAtUtc = DateTime.UtcNow;

        Event @event = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Address.StreetName(),
            startsAtUtc,
            null);

        // Act
        Result<Ticket> result = Ticket.Create(
            Guid.CreateVersion7(),
            attendee,
            @event,
            Faker.Random.AlphaNumeric(10));

        // Assert
        TicketCreatedDomainEvent domainEvent = AssertDomainEventWasPublished<TicketCreatedDomainEvent>(result.Value);
        Assert.Equal(result.Value.Id, domainEvent.TicketId);
    }

    [Fact]
    public void MarkAsUsed_ShouldRaiseDomainEvent_WhenTicketIsUsed()
    {
        // Arrange
        Attendee attendee = Attendee.Create(
            Guid.CreateVersion7(),
            Faker.Internet.Email(),
            Faker.Person.FirstName,
            Faker.Person.LastName);

        DateTime startsAtUtc = DateTime.UtcNow;

        Event @event = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Address.StreetName(),
            startsAtUtc,
            null);

        Ticket ticket = Ticket.Create(
            Guid.CreateVersion7(),
            attendee,
            @event,
            Faker.Random.AlphaNumeric(10));

        // Act
        ticket.MarkAsUsed();

        // Assert
        TicketUsedDomainEvent domainEvent = AssertDomainEventWasPublished<TicketUsedDomainEvent>(ticket);
        Assert.Equal(ticket.Id, domainEvent.TicketId);
    }
}
