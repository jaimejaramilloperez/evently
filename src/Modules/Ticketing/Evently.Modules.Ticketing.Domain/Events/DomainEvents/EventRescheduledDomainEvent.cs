using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Events.DomainEvents;

public sealed class EventRescheduledDomainEvent(Guid eventId, DateTime startsAtUtc, DateTime? endsAtUtc)
    : DomainEvent
{
    public Guid EventId => eventId;
    public DateTime StartsAtUtc => startsAtUtc;
    public DateTime? EndsAtUtc => endsAtUtc;
}
