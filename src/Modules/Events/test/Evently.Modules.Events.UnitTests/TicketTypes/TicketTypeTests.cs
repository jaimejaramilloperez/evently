using Evently.Common.Domain.Results;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Domain.TicketTypes.DomainEvents;
using Evently.Modules.Events.UnitTests.Abstractions;
using Microsoft.Extensions.Time.Testing;

namespace Evently.Modules.Events.UnitTests.TicketTypes;

public class TicketTypeTests : BaseTest
{
    private readonly FakeTimeProvider _timeProvider = new();
    private readonly Event _event;

    public TicketTypeTests()
    {
        Category category = Category.Create(Faker.Random.AlphaNumeric(10));
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> eventResult = Event.Create(
            _timeProvider,
            category.Id,
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.AlphaNumeric(10),
            startsAtUtc,
            null);

        _event = eventResult.Value;
    }

    [Fact]
    public void Create_ShouldReturnValue_WhenTicketTypeIsCreated()
    {
        // Arrange

        // Act
        Result<TicketType> sut = TicketType.Create(
            _event.Id,
            Faker.Random.AlphaNumeric(10),
            Faker.Random.Decimal(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.Decimal());

        // Assert
        Assert.NotNull(sut);
        Assert.True(sut.IsSuccess);
        Assert.NotNull(sut.Value);
    }

    [Fact]
    public void UpdatePrice_ShouldRaiseDomainEvent_WhenTicketTypeIsUpdated()
    {
        // Arrange
        Result<TicketType> result = TicketType.Create(
            _event.Id,
            Faker.Random.AlphaNumeric(10),
            Faker.Random.Decimal(),
            Faker.Random.AlphaNumeric(10),
            Faker.Random.Decimal());

        TicketType sut = result.Value;

        // Act
        sut.UpdatePrice(Faker.Random.Decimal());

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        TicketTypePriceChangedDomainEvent domainEvent = AssertDomainEventWasPublished<TicketTypePriceChangedDomainEvent>(sut);
        Assert.Equivalent(sut.Id, domainEvent.TicketTypeId);
    }
}
