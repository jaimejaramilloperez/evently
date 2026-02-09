using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Events.CreateEvent;

public sealed record CreateEventCommand : ICommand
{
    public required Guid EventId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Location { get; init; }
    public required DateTime StartsAtUtc { get; init; }
    public required DateTime? EndsAtUtc { get; init; }
    public required ICollection<TicketTypeRequest> TicketTypes { get; init; }

    public sealed record TicketTypeRequest(
        Guid TicketTypeId,
        Guid EventId,
        string Name,
        decimal Price,
        string Currency,
        decimal Quantity);
}
