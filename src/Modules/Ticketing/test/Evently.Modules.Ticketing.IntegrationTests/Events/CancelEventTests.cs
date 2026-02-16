using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Events.CancelEvent;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Modules.Ticketing.IntegrationTests.Events;

public class CancelEventTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private const decimal Quantity = 10;

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange
        Guid eventId = Guid.CreateVersion7();

        CancelEventCommand command = new(eventId);

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.NotFound(command.EventId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventIsCanceled()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid eventId = Guid.CreateVersion7();
        Guid ticketTypeId = Guid.CreateVersion7();

        await CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity, cancellationToken);

        CancelEventCommand command = new(eventId);

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
