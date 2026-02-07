using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Attendance.Domain.Attendees.DomainEvents;

public sealed class DuplicateCheckInAttemptedDomainEvent(
    Guid attendeeId,
    Guid eventId,
    Guid ticketId,
    string ticketCode)
    : DomainEvent
{
    public Guid AttendeeId => attendeeId;
    public Guid EventId => eventId;
    public Guid TicketId => ticketId;
    public string TicketCode => ticketCode;
}
