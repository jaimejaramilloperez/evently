using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class EventRescheduledIntegrationEvent : IntegrationEvent
{
    public Guid EventId { get; init; }
    public DateTime StartsAtUtc { get; init; }
    public DateTime? EndsAtUtc { get; init; }

    public EventRescheduledIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid eventId,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
        : base(id, occurredAtUtc)
    {
        EventId = eventId;
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
    }
}
