using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Attendance.Domain.Tickets.DomainEvents;

public sealed class TicketUsedDomainEvent(Guid ticketId) : DomainEvent
{
    public Guid TicketId => ticketId;
}
