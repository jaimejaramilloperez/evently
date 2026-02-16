using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Events.DomainEvents;
using Evently.Modules.Attendance.UnitTests.Abstractions;

namespace Evently.Modules.Attendance.UnitTests.Events;

public class EventTests : BaseTest
{
    [Fact]
    public void Should_RaiseDomainEvent_WhenEventCreated()
    {
        // Arrange
        DateTime startsAtUtc = DateTime.Now;

        // Act
        Result<Event> result = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        // Assert
        EventCreatedDomainEvent domainEvent = AssertDomainEventWasPublished<EventCreatedDomainEvent>(result.Value);
        Assert.Equal(result.Value.Id, domainEvent.EventId);
    }
}
