using Evently.Modules.Events.Domain.Abstractions.DomainEvents;

namespace Evently.Modules.Events.Domain.Events.DomainEvents;

public sealed class EventPublishedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}
