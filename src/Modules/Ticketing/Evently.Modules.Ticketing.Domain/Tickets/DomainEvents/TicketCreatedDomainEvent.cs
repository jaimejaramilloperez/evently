using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;

public sealed class TicketCreatedDomainEvent(Guid ticketId) : DomainEvent
{
    public Guid TicketId => ticketId;
}
