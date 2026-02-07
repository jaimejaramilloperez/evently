using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.CheckInAttendee;

public sealed record CheckInAttendeeCommand : ICommand
{
    public required Guid AttendeeId { get; init; }
    public required Guid TicketId { get; init; }
}
