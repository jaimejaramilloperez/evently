using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Events.RescheduleEvent;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Modules.Ticketing.IntegrationTests.Events;

public class RescheduleEventTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private const decimal Quantity = 10;

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange
        Guid eventId = Guid.NewGuid();
        DateTime startsAtUtc = DateTime.UtcNow;
        DateTime endsAtUtc = startsAtUtc.AddHours(1);

        RescheduleEventCommand command = new()
        {
            EventId = eventId,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.NotFound(command.EventId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventAlreadyStarted()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid eventId = Guid.NewGuid();
        Guid ticketTypeId = Guid.NewGuid();
        DateTime startsAtUtc = DateTime.UtcNow.AddMinutes(-5);
        DateTime endsAtUtc = startsAtUtc.AddHours(1);

        await CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity, cancellationToken);

        RescheduleEventCommand command = new()
        {
            EventId = eventId,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.StartDateInPast, result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenEventRescheduled()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid eventId = Guid.NewGuid();
        Guid ticketTypeId = Guid.NewGuid();
        DateTime startsAtUtc = DateTime.UtcNow;
        DateTime endsAtUtc = startsAtUtc.AddHours(1);

        await CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity, cancellationToken);

        RescheduleEventCommand command = new()
        {
            EventId = eventId,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.StartDateInPast, result.Error);
    }
}
