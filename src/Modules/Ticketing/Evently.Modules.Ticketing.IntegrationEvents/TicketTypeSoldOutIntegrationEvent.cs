using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class TicketTypeSoldOutIntegrationEvent : IntegrationEvent
{
    public Guid TicketTypeId { get; init; }

    public TicketTypeSoldOutIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid ticketTypeId)
        : base(id, occurredAtUtc)
    {
        TicketTypeId = ticketTypeId;
    }
}
