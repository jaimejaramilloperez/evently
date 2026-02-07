using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class TicketTypePriceChangedIntegrationEvent : IntegrationEvent
{
    public Guid TicketTypeId { get; init; }
    public decimal Price { get; init; }

    public TicketTypePriceChangedIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid ticketTypeId,
        decimal price)
        : base(id, occurredAtUtc)
    {
        TicketTypeId = ticketTypeId;
        Price = price;
    }
}
