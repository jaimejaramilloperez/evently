using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Events.GetEvent;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.Events;

public class GetEventTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange
        GetEventQuery query = new(Guid.NewGuid());

        // Act
        Result<EventResponse?> result = await SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.NotFound(query.EventId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnEvent_WhenEventExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        await CleanDatabaseAsync(cancellationToken);

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);
        Guid eventId = await CreateEventAsync(categoryId, cancellationToken: cancellationToken);

        GetEventQuery query = new(eventId);

        // Act
        Result<EventResponse?> result = await SendAsync(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}
