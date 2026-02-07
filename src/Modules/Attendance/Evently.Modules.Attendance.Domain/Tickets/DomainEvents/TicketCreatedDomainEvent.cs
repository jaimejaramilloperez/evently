using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Attendance.Domain.Tickets.DomainEvents;

public sealed class TicketCreatedDomainEvent(Guid ticketId, Guid eventId) : DomainEvent
{
    public Guid TicketId => ticketId;
    public Guid EventId => eventId;
}
