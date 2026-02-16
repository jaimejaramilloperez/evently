using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.TicketTypes;

public class GetTicketTypeTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange
        GetTicketTypeQuery query = new(Guid.CreateVersion7());

        // Act
        Result<TicketTypeResponse> result = await SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(TicketTypeErrors.NotFound(query.TicketTypeId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnTicketType_WhenTicketTypeExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        await CleanDatabaseAsync(cancellationToken);

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);
        Guid eventId = await CreateEventAsync(categoryId, cancellationToken: cancellationToken);

        Guid ticketTypeId = await CreateTicketTypeAsync(eventId, cancellationToken);

        GetTicketTypeQuery query = new(ticketTypeId);

        // Act
        Result<TicketTypeResponse> result = await SendAsync(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}
