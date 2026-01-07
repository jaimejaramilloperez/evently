namespace Evently.Modules.Events.Events.Models;

public sealed class Event
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Location { get; set; }
    public required DateTime StartsAtUtc { get; set; }
    public DateTime? EndsAtUtc { get; set; }
    public required EventStatus Status { get; set; }
}
