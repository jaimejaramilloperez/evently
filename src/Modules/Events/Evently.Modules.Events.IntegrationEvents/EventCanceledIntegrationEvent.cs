using Evently.Common.Application.EventBus;

namespace Evently.Modules.Events.IntegrationEvents;

public sealed class EventCanceledIntegrationEvent : IntegrationEvent
{
    public Guid EventId { get; init; }

    public EventCanceledIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid eventId)
        : base(id, occurredAtUtc)
    {
        EventId = eventId;
    }
}
