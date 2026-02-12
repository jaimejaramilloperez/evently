using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class EventTicketsArchivedIntegrationEvent : IntegrationEvent
{
    public Guid EventId { get; init; }

    public EventTicketsArchivedIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid eventId)
        : base(id, occurredAtUtc)
    {
        EventId = eventId;
    }
}
