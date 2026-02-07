using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.UpdateAttendee;

public sealed record UpdateAttendeeCommand : ICommand
{
    public required Guid AttendeeId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
