using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.IntegrationTests.Abstractions;

namespace Evently.Modules.Events.IntegrationTests.TicketTypes;

public class UpdateTicketTypeTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange
        UpdateTicketTypePriceCommand command = new()
        {
            TicketTypeId = Guid.CreateVersion7(),
            Price = Faker.Random.Decimal(),
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(TicketTypeErrors.NotFound(command.TicketTypeId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenTicketTypeExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid categoryId = await CreateCategoryAsync(Faker.Random.AlphaNumeric(10), cancellationToken);
        Guid eventId = await CreateEventAsync(categoryId, cancellationToken: cancellationToken);
        Guid ticketTypeId = await CreateTicketTypeAsync(eventId, cancellationToken);

        UpdateTicketTypePriceCommand command = new()
        {
            TicketTypeId = ticketTypeId,
            Price = Faker.Random.Decimal(),
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
