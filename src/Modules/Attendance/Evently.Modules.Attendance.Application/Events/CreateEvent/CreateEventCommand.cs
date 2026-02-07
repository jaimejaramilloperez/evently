using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Events.CreateEvent;

public sealed record CreateEventCommand : ICommand
{
    public required Guid EventId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Location { get; init; }
    public required DateTime StartsAtUtc { get; init; }
    public DateTime? EndsAtUtc { get; init; }
}
