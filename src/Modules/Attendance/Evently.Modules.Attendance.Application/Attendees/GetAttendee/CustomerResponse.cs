namespace Evently.Modules.Attendance.Application.Attendees.GetAttendee;

public sealed record AttendeeResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
