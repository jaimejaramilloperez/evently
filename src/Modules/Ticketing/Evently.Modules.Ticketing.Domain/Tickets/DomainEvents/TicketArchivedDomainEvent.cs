using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;

public sealed class TicketArchivedDomainEvent(Guid ticketId, string code) : DomainEvent
{
    public Guid TicketId => ticketId;
    public string Code => code;
}
