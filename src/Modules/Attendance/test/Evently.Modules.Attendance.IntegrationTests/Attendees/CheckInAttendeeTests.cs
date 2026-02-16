using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Attendees.CheckInAttendee;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.IntegrationTests.Abstractions;

namespace Evently.Modules.Attendance.IntegrationTests.Attendees;

public class CheckInAttendeeTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenAttendeeDoesNotExist()
    {
        // Arrange
        CheckInAttendeeCommand command = new()
        {
            AttendeeId = Guid.CreateVersion7(),
            TicketId = Guid.CreateVersion7(),
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(AttendeeErrors.NotFound(command.AttendeeId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid attendeeId = await CreateAttendeeAsync(Guid.CreateVersion7(), cancellationToken);

        CheckInAttendeeCommand command = new()
        {
            AttendeeId = attendeeId,
            TicketId = Guid.CreateVersion7(),
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(TicketErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenAttendeeCheckedIn()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid attendeeId = await CreateAttendeeAsync(Guid.CreateVersion7(), cancellationToken);
        Guid eventId = await CreateEventAsync(Guid.CreateVersion7(), cancellationToken);
        Guid ticketId = await CreateTicketAsync(Guid.CreateVersion7(), attendeeId, eventId, cancellationToken);

        CheckInAttendeeCommand command = new()
        {
            AttendeeId = attendeeId,
            TicketId = ticketId,
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
