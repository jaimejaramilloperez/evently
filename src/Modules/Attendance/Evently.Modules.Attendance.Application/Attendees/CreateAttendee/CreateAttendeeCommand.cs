using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Attendees.CreateAttendee;

public sealed record CreateAttendeeCommand : ICommand
{
    public required Guid AttendeeId { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
