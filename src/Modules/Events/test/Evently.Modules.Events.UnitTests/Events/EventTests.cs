using Evently.Common.Domain.Results;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.Events.DomainEvents;
using Evently.Modules.Events.UnitTests.Abstractions;
using Microsoft.Extensions.Time.Testing;

namespace Evently.Modules.Events.UnitTests.Events;

public class EventTests : BaseTest
{
    private readonly FakeTimeProvider _timeProvider = new();

    [Fact]
    public void Create_ShouldReturnFailure_WhenEndDatePrecedesStartDate()
    {
        // Arrange
        Category category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        DateTime endsAtUtc = startsAtUtc.AddMinutes(-1);

        // Act
        Result<Event> sut = Event.Create(
            _timeProvider,
            category.Id,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            endsAtUtc);

        // Assert
        Assert.NotNull(sut);
        Assert.False(sut.IsSuccess);
        Assert.Equivalent(EventErrors.EndDatePrecedesStartDate, sut.Error);
    }

    [Fact]
    public void Create_ShouldRaiseADomainEvent_WhenEventIsCreated()
    {
        // Arrange
        Category category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        // Act
        Result<Event> result = Event.Create(
            _timeProvider,
            category.Id,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event sut = result.Value;

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        EventCreatedDomainEvent domainEvent = AssertDomainEventWasPublished<EventCreatedDomainEvent>(sut);
        Assert.Equal(sut.Id, domainEvent.EventId);
    }

    [Fact]
    public void Publish_ShouldReturnFailure_WhenEventNotDraft()
    {
        // Arrange
        Category category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            _timeProvider,
            category.Id,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event sut = result.Value;

        sut.Publish();

        // Act
        Result publishResult = sut.Publish();

        // Assert
        Assert.NotNull(publishResult);
        Assert.False(publishResult.IsSuccess);
        Assert.Equivalent(EventErrors.NotDraft, publishResult.Error);
    }


    [Fact]
    public void Publish_ShouldRaiseDomainEvent_WhenEventPublished()
    {
        // Arrange
        Category category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            _timeProvider,
            category.Id,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event sut = result.Value;

        // Act
        sut.Publish();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        EventPublishedDomainEvent domainEvent = AssertDomainEventWasPublished<EventPublishedDomainEvent>(sut);
        Assert.Equal(sut.Id, domainEvent.EventId);
    }

    [Fact]
    public void Reschedule_ShouldRaiseDomainEvent_WhenEventRescheduled()
    {
        // Arrange
        Category category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;

        Result<Event> result = Event.Create(
            _timeProvider,
            category.Id,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event sut = result.Value;

        // Act
        sut.Reschedule(_timeProvider, startsAtUtc.AddDays(1), startsAtUtc.AddDays(2));

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        EventRescheduledDomainEvent domainEvent = AssertDomainEventWasPublished<EventRescheduledDomainEvent>(sut);
        Assert.Equal(sut.Id, domainEvent.EventId);
    }

    [Fact]
    public void Cancel_ShouldRaiseDomainEvent_WhenEventCanceled()
    {
        // Arrange
        Category category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        _timeProvider.SetUtcNow(startsAtUtc.AddMinutes(-1));

        Result<Event> result = Event.Create(
            _timeProvider,
            category.Id,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event sut = result.Value;

        // Act
        sut.Cancel(_timeProvider);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        EventCanceledDomainEvent domainEvent = AssertDomainEventWasPublished<EventCanceledDomainEvent>(sut);
        Assert.Equal(sut.Id, domainEvent.EventId);
    }

    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyCanceled()
    {
        // Arrange
        Category category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        _timeProvider.SetUtcNow(startsAtUtc.AddMinutes(-1));

        Result<Event> result = Event.Create(
            _timeProvider,
            category.Id,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event sut = result.Value;

        sut.Cancel(_timeProvider);

        // Act
        Result cancelResult = sut.Cancel(_timeProvider);

        // Assert
        Assert.NotNull(cancelResult);
        Assert.False(cancelResult.IsSuccess);
        Assert.Equivalent(EventErrors.AlreadyCanceled, cancelResult.Error);
    }

    [Fact]
    public void Cancel_ShouldReturnFailure_WhenEventAlreadyStarted()
    {
        // Arrange
        Category category = Category.Create(Faker.Music.Genre());
        DateTime startsAtUtc = DateTime.UtcNow;
        _timeProvider.SetUtcNow(startsAtUtc.AddMinutes(1));

        Result<Event> result = Event.Create(
            _timeProvider,
            category.Id,
            Faker.Music.Genre(),
            Faker.Music.Genre(),
            Faker.Address.StreetAddress(),
            startsAtUtc,
            null);

        Event sut = result.Value;

        // Act
        Result cancelResult = sut.Cancel(_timeProvider);

        // Assert
        Assert.NotNull(cancelResult);
        Assert.False(cancelResult.IsSuccess);
        Assert.Equivalent(EventErrors.AlreadyStarted, cancelResult.Error);
    }

}
