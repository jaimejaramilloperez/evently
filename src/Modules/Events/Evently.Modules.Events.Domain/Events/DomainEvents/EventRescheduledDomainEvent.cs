using Evently.Modules.Events.Domain.Abstractions.DomainEvents;

namespace Evently.Modules.Events.Domain.Events.DomainEvents;

public sealed class EventRescheduledDomainEvent(Guid eventId, DateTime startsAtUtc, DateTime? endsAtUtc)
    : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
    public DateTime StartsAtUtc { get; init; } = startsAtUtc;
    public DateTime? EndsAtUtc { get; init; } = endsAtUtc;
}
