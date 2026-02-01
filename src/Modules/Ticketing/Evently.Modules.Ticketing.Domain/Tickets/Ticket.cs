using Evently.Common.Domain;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Tickets;

public sealed class Ticket : Entity
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid EventId { get; private set; }
    public Guid TicketTypeId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public DateTime CreatedAtUtc { get; private set; }
    public bool Archived { get; private set; }

    public static Guid CreateTicketId()
    {
        return Guid.CreateVersion7();
    }

    public static Ticket Create(Order order, TicketType ticketType)
    {
        Ticket ticket = new()
        {
            Id = CreateTicketId(),
            CustomerId = order.CustomerId,
            OrderId = order.Id,
            EventId = ticketType.EventId,
            TicketTypeId = ticketType.Id,
            Code = $"tc_{Guid.CreateVersion7()}",
            CreatedAtUtc = DateTime.UtcNow,
        };

        ticket.RaiseEvent(new TicketCreatedDomainEvent(ticket.Id));

        return ticket;
    }

    private Ticket()
    {
    }

    public Result Archive()
    {
        if (Archived)
        {
            return Result.Success();
        }

        Archived = true;

        RaiseEvent(new TicketArchivedDomainEvent(Id, Code));

        return Result.Success();
    }
}
