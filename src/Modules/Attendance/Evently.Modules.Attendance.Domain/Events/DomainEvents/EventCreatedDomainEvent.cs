using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Attendance.Domain.Events.DomainEvents;

public sealed class EventCreatedDomainEvent(
    Guid eventId,
    string title,
    string description,
    string location,
    DateTime startsAtUtc,
    DateTime? endsAtUtc) : DomainEvent
{
    public Guid EventId => eventId;
    public string Title => title;
    public string Description => description;
    public string Location => location;
    public DateTime StartsAtUtc => startsAtUtc;
    public DateTime? EndsAtUtc => endsAtUtc;
}
