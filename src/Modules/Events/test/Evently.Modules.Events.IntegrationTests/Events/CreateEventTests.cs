using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Events.CreateEvent;
using Evently.Modules.Events.Application.Events.GetEvents;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.Events;

public class CreateEventTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenStartDateInPast()
    {
        // Arrange
        DateTime baseTime = new(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        FakeTimeProvider.SetUtcNow(baseTime);

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), TestContext.Current.CancellationToken);

        CreateEventCommand command = new()
        {
            CategoryId = categoryId,
            Title = Faker.Random.AlphaNumeric(10),
            Description = Faker.Random.AlphaNumeric(10),
            Location = Faker.Random.AlphaNumeric(10),
            StartsAtUtc = FakeTimeProvider.GetUtcNow().AddMinutes(-10).UtcDateTime,
            EndsAtUtc = null,
        };

        // Act
        Result<EventResponse> result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        ValidationError? validationError = result.Error as ValidationError;
        Assert.NotNull(validationError);
        Assert.Contains(validationError.Errors, e => e.Code == EventErrors.StartDateInPast.Code);

        FakeTimeProvider.SetUtcNow(DateTimeOffset.UtcNow);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCategoryDoesNotExist()
    {
        // Arrange
        Guid categoryId = Guid.CreateVersion7();

        CreateEventCommand command = new()
        {
            CategoryId = categoryId,
            Title = Faker.Random.AlphaNumeric(10),
            Description = Faker.Random.AlphaNumeric(10),
            Location = Faker.Random.AlphaNumeric(10),
            StartsAtUtc = DateTime.UtcNow.AddMinutes(10),
            EndsAtUtc = null,
        };

        // Act
        Result<EventResponse> result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(CategoryErrors.NotFound(categoryId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEndDatePrecedesStartDate()
    {
        // Arrange
        DateTime startsAtUtc = DateTime.UtcNow.AddMinutes(10);
        DateTime endsAtUtc = startsAtUtc.AddMinutes(-5);

        CreateEventCommand command = new()
        {
            CategoryId = Guid.CreateVersion7(),
            Title = Faker.Random.AlphaNumeric(10),
            Description = Faker.Random.AlphaNumeric(10),
            Location = Faker.Random.AlphaNumeric(10),
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
        };

        // Act
        Result<EventResponse> result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(ErrorType.Validation, result.Error.Type);
    }

    [Fact]
    public async Task Should_CreateEvent_WhenCommandIsValid()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        await CleanDatabaseAsync(cancellationToken);
        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);

        CreateEventCommand command = new()
        {
            CategoryId = categoryId,
            Title = Faker.Random.AlphaNumeric(10),
            Description = Faker.Random.AlphaNumeric(10),
            Location = Faker.Random.AlphaNumeric(10),
            StartsAtUtc = DateTime.UtcNow.AddMinutes(10),
            EndsAtUtc = null,
        };

        // Act
        Result<EventResponse> result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}
