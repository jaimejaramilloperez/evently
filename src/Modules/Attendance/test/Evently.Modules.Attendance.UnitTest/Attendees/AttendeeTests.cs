using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.UnitTests.Abstractions;

namespace Evently.Modules.Attendance.UnitTests.Attendees;

public class AttendeeTests : BaseTest
{
    [Fact]
    public void CheckIn_ShouldReturnFailure_WhenTicketIsNotValid()
    {
        // Arrange
        Attendee attendee = Attendee.Create(
            Guid.CreateVersion7(),
            Faker.Internet.Email(),
            Faker.Person.FirstName,
            Faker.Person.LastName);

        Attendee ticketAttendee = Attendee.Create(
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
            ticketAttendee,
            @event,
            Faker.Random.AlphaNumeric(10));

        // Act
        Result checkInAttendee = attendee.CheckIn(ticket);

        // Assert
        InvalidCheckInAttemptedDomainEvent domainEvent = AssertDomainEventWasPublished<InvalidCheckInAttemptedDomainEvent>(attendee);

        Assert.Equal(attendee.Id, domainEvent.AttendeeId);
        Assert.Equivalent(TicketErrors.InvalidCheckIn, checkInAttendee.Error);
    }

    [Fact]
    public void CheckIn_ShouldReturnFailure_WhenTicketAlreadyUsed()
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

        ticket.MarkAsUsed();

        // Act
        Result checkInAttendee = attendee.CheckIn(ticket);

        // Assert
        DuplicateCheckInAttemptedDomainEvent domainEvent = AssertDomainEventWasPublished<DuplicateCheckInAttemptedDomainEvent>(attendee);

        Assert.Equal(attendee.Id, domainEvent.AttendeeId);
        Assert.Equivalent(TicketErrors.DuplicateCheckIn, checkInAttendee.Error);
    }

    [Fact]
    public void CheckIn_ShouldRaiseDomainEvent_WhenSuccessfullyCheckedIn()
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
        attendee.CheckIn(ticket);

        // Assert
        AttendeeCheckedInDomainEvent domainEvent = AssertDomainEventWasPublished<AttendeeCheckedInDomainEvent>(attendee);
        Assert.Equal(attendee.Id, domainEvent.AttendeeId);
    }
}
