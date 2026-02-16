using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.Application.TicketTypes.GetTicketTypes;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.TicketTypes;

public class GetTicketTypesTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypesDoNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        await CleanDatabaseAsync(cancellationToken);

        var query = new GetTicketTypesQuery(Guid.NewGuid());

        // Act
        Result<IReadOnlyCollection<TicketTypeResponse>> result = await SendAsync(query, cancellationToken);

        // Assert
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task Should_ReturnTicketTypes_WhenTicketTypesExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        await CleanDatabaseAsync(cancellationToken);

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);
        Guid eventId = await CreateEventAsync(categoryId, cancellationToken: cancellationToken);

        await CreateTicketTypeAsync(eventId, cancellationToken);
        await CreateTicketTypeAsync(eventId, cancellationToken);

        GetTicketTypesQuery query = new(eventId);

        // Act
        Result<IReadOnlyCollection<TicketTypeResponse>> result = await SendAsync(query, cancellationToken);

        // Assert
        Assert.Equal(2, result.Value.Count);
    }
}
