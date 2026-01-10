using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Events.Domain.TicketTypes.DomainEvents;

public sealed class TicketTypePriceChangedDomainEvent(Guid ticketTypeId, decimal price)
    : DomainEvent
{
    public Guid TicketTypeId { get; init; } = ticketTypeId;
    public decimal Price { get; init; } = price;
}
