using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;
using Evently.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Modules.Ticketing.UnitTests.Events;

public class TicketTypeTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnValue_WhenTicketTypeIsCreated()
    {
        // Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Event @event = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        // Act
        Result<TicketType> result = TicketType.Create(
            Guid.CreateVersion7(),
            @event.Id,
            Faker.Name.FirstName(),
            Faker.Random.Decimal(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.Decimal());

        // Assert
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void UpdateQuantity_ShouldReturnFailure_WhenNotEnoughQuantity()
    {
        // Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Event @event = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        decimal quantity = Faker.Random.Decimal();

        TicketType ticketType = TicketType.Create(
            Guid.CreateVersion7(),
            @event.Id,
            Faker.Name.FirstName(),
            Faker.Random.Decimal(),
            Faker.Random.AlphaNumeric(10),
            quantity);

        // Act
        Result result = ticketType.UpdateQuantity(quantity + 1);

        // Assert
        Assert.Equivalent(TicketTypeErrors.NotEnoughQuantity(quantity), result.Error);
    }

    [Fact]
    public void UpdateQuantity_ShouldRaiseDomainEvent_WhenTicketTypesIsSoldOut()
    {
        // Arrange
        DateTime startsAtUtc = DateTime.UtcNow;

        Event @event = Event.Create(
            Guid.CreateVersion7(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        decimal quantity = Faker.Random.Decimal();

        Result<TicketType> ticketType = TicketType.Create(
            Guid.CreateVersion7(),
            @event.Id,
            Faker.Name.FirstName(),
            Faker.Random.Decimal(),
            Faker.Random.AlphaNumeric(10),
            quantity);

        // Act
        ticketType.Value.UpdateQuantity(quantity);

        // Assert
        TicketTypeSoldOutDomainEvent domainEvent = AssertDomainEventWasPublished<TicketTypeSoldOutDomainEvent>(ticketType.Value);
        Assert.Equal(ticketType.Value.Id, domainEvent.TicketTypeId);
    }
}
