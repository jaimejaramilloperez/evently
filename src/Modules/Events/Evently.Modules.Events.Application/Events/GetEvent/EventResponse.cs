namespace Evently.Modules.Events.Application.Events.GetEvent;

public sealed record EventResponse
{
    public required Guid Id { get; init; }
    public required Guid CategoryId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Location { get; init; }
    public required DateTime StartsAtUtc { get; init; }
    public DateTime? EndsAtUtc { get; init; }
    public IList<TicketTypeResponse> TicketTypes { get; init; } = [];
}
