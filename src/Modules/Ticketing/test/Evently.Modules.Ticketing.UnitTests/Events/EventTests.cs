using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;
using Evently.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Modules.Ticketing.UnitTests.Events;

public class EventTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnValue_WhenEventIsCreated()
    {
        // Act
        Result<Event> @event = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            DateTime.UtcNow,
            null);

        // Assert
        Assert.NotNull(@event.Value);
    }

    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventIsRescheduled()
    {
        // Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> @event = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        // Act
        @event.Value.Reschedule(startsAtUtc.AddDays(1), startsAtUtc.AddDays(2));

        // Assert
        EventRescheduledDomainEvent domainEvent = AssertDomainEventWasPublished<EventRescheduledDomainEvent>(@event.Value);
        Assert.Equal(@event.Value.Id, domainEvent.EventId);
    }

    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventIsCanceled()
    {
        // Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> @event = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        // Act
        @event.Value.Cancel();

        // Assert
        EventCanceledDomainEvent domainEvent = AssertDomainEventWasPublished<EventCanceledDomainEvent>(@event.Value);
        Assert.Equal(@event.Value.Id, domainEvent.EventId);
    }

    [Fact]
    public void PaymentsRefunded_ShouldRaiseDomainEvent_WhenPaymentsAreRefunded()
    {
        // Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> @event = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        // Act
        @event.Value.PaymentsRefunded();

        // Assert
        EventPaymentsRefundedDomainEvent domainEvent = AssertDomainEventWasPublished<EventPaymentsRefundedDomainEvent>(@event.Value);
        Assert.Equal(@event.Value.Id, domainEvent.EventId);

    }

    [Fact]
    public void TicketsArchived_ShouldRaiseDomainEvent_WhenTicketsAreArchived()
    {
        // Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> @event = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        // Act
        @event.Value.TicketsArchived();

        // Assert
        EventTicketsArchivedDomainEvent domainEvent = AssertDomainEventWasPublished<EventTicketsArchivedDomainEvent>(@event.Value);
        Assert.Equal(@event.Value.Id, domainEvent.EventId);

    }
}
