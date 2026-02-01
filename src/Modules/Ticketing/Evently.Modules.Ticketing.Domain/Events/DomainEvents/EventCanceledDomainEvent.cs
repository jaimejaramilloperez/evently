using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Events.DomainEvents;

public sealed class EventCanceledDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId => eventId;
}
