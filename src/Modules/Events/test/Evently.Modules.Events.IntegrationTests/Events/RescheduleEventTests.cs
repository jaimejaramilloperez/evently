using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Events.PublishEvent;
using Evently.Modules.Events.Application.Events.RescheduleEvent;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.Events;

public class RescheduleEventTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange
        Guid eventId = Guid.CreateVersion7();

        PublishEventCommand command = new(eventId);

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.NotFound(eventId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenStartDateIsInPast()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);
        Guid eventId = await CreateEventAsync(categoryId, cancellationToken: cancellationToken);

        DateTimeOffset baseTime = FakeTimeProvider.GetUtcNow().AddHours(1);
        FakeTimeProvider.SetUtcNow(baseTime);

        RescheduleEventCommand command = new()
        {
            EventId = eventId,
            StartsAtUtc = baseTime.AddMinutes(-10).UtcDateTime,
            EndsAtUtc = null,
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        ValidationError? validationError = result.Error as ValidationError;
        Assert.NotNull(validationError);
        Assert.Contains(validationError.Errors, e => e.Code == EventErrors.StartDateInPast.Code);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsRescheduled()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);
        Guid eventId = await CreateEventAsync(categoryId, cancellationToken: cancellationToken);

        DateTimeOffset baseTime = FakeTimeProvider.GetUtcNow().AddMinutes(1);
        FakeTimeProvider.SetUtcNow(baseTime);

        RescheduleEventCommand command = new()
        {
            EventId = eventId,
            StartsAtUtc = baseTime.AddHours(1).UtcDateTime,
            EndsAtUtc = null,
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
