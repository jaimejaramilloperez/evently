using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Attendance.Domain.Attendees.DomainEvents;

public sealed class AttendeeCheckedInDomainEvent(Guid attendeeId, Guid eventId) : DomainEvent
{
    public Guid AttendeeId => attendeeId;
    public Guid EventId => eventId;
}
