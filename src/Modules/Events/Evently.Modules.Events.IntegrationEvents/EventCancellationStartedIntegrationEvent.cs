using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class EventCancellationStartedIntegrationEvent : IntegrationEvent
{
    public Guid EventId { get; init; }

    public EventCancellationStartedIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid eventId)
        : base(id, occurredAtUtc)
    {
        EventId = eventId;
    }
}
