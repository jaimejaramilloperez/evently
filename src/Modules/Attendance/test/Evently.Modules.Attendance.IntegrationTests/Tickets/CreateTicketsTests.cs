using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Tickets.CreateTicket;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.IntegrationTests.Abstractions;

namespace Evently.Modules.Attendance.IntegrationTests.Tickets;

public class CreateTicketsTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
    {
        // Arrange
        CreateTicketCommand command = new()
        {
            TicketId = Guid.CreateVersion7(),
            AttendeeId = Guid.CreateVersion7(),
            EventId = Guid.CreateVersion7(),
            Code = Guid.CreateVersion7().ToString(),
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(AttendeeErrors.NotFound(command.AttendeeId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenEventDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid attendeeId = await CreateAttendeeAsync(Guid.CreateVersion7(), cancellationToken);

        CreateTicketCommand command = new()
        {
            TicketId = Guid.CreateVersion7(),
            AttendeeId = attendeeId,
            EventId = Guid.CreateVersion7(),
            Code = Guid.CreateVersion7().ToString(),
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(EventErrors.NotFound(command.EventId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenTicketIsCreated()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid attendeeId = await CreateAttendeeAsync(Guid.CreateVersion7(), cancellationToken);
        Guid eventId = await CreateEventAsync(Guid.CreateVersion7(), cancellationToken);

        CreateTicketCommand command = new()
        {
            TicketId = Guid.CreateVersion7(),
            AttendeeId = attendeeId,
            EventId = eventId,
            Code = Guid.CreateVersion7().ToString(),
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
