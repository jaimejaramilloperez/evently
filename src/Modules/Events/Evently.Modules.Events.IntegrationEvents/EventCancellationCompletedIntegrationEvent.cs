using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class EventCancellationCompletedIntegrationEvent : IntegrationEvent
{
    public Guid EventId { get; init; }

    public EventCancellationCompletedIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid eventId)
        : base(id, occurredAtUtc)
    {
        EventId = eventId;
    }
}
