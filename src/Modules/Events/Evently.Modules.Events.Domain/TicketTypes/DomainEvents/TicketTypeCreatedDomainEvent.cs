using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Events.Domain.TicketTypes.DomainEvents;

public sealed class TicketTypeCreatedDomainEvent(Guid ticketTypeId) : DomainEvent
{
    public Guid TicketTypeId => ticketTypeId;
}
