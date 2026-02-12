using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class EventPaymentsRefundedIntegrationEvent : IntegrationEvent
{
    public Guid EventId { get; init; }

    public EventPaymentsRefundedIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid eventId)
        : base(id, occurredAtUtc)
    {
        EventId = eventId;
    }
}
