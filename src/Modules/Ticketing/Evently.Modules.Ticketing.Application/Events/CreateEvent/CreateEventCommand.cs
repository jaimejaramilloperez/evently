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

    public sealed record TicketTypeRequest
    {
        public required Guid TicketTypeId { get; init; }
        public required Guid EventId { get; init; }
        public required string Name { get; init; }
        public required decimal Price { get; init; }
        public required string Currency { get; init; }
        public required decimal Quantity { get; init; }
    }
}
