using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Events.CancelEvent;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.Events;

public class CancelEventTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange
        Guid eventId = Guid.CreateVersion7();
        CancelEventCommand command = new(eventId);

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.NotFound(eventId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventAlreadyCanceled()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);
        Guid eventId = await CreateEventAsync(categoryId, cancellationToken: cancellationToken);

        CancelEventCommand command = new(eventId);

        await SendAsync(command, cancellationToken);

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.AlreadyCanceled, result.Error);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventAlreadyStarted()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        DateTimeOffset baseTime = FakeTimeProvider.GetUtcNow().AddHours(1);
        FakeTimeProvider.SetUtcNow(baseTime);

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);

        DateTime eventStartTime = baseTime.UtcDateTime.AddMinutes(5);
        Guid eventId = await CreateEventAsync(categoryId, eventStartTime, cancellationToken);

        FakeTimeProvider.SetUtcNow(baseTime.AddMinutes(15));

        CancelEventCommand command = new(eventId);

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.AlreadyStarted, result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsCanceled()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);
        Guid eventId = await CreateEventAsync(categoryId, cancellationToken: cancellationToken);

        CancelEventCommand command = new(eventId);

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
