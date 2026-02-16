using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Events.GetEvents;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.Events;

public class GetEventsTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenEventsDoNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        await CleanDatabaseAsync(cancellationToken);

        GetEventsQuery query = new();

        // Act
        Result<IReadOnlyCollection<EventResponse>> result = await SendAsync(query, cancellationToken);

        // Assert
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Should_ReturnEvents_WhenEventsExist()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        await CleanDatabaseAsync(cancellationToken);

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);
        await CreateEventAsync(categoryId, cancellationToken: cancellationToken);
        await CreateEventAsync(categoryId, cancellationToken: cancellationToken);

        GetEventsQuery query = new();

        // Act
        Result<IReadOnlyCollection<EventResponse>> result = await SendAsync(query, cancellationToken);

        // Assert
        Assert.Equal(2, result.Value.Count);
    }
}
