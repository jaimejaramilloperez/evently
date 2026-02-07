using Evently.Common.Domain;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets.DomainEvents;

namespace Evently.Modules.Attendance.Domain.Tickets;

public sealed class Ticket : Entity
{
    public Guid Id { get; private set; }
    public Guid AttendeeId { get; private set; }
    public Guid EventId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public DateTime? UsedAtUtc { get; private set; }

    public static Ticket Create(
        Guid ticketId,
        Attendee attendee,
        Event @event,
        string code)
    {
        Ticket ticket = new()
        {
            Id = ticketId,
            AttendeeId = attendee.Id,
            EventId = @event.Id,
            Code = code,
        };

        ticket.RaiseEvent(new TicketCreatedDomainEvent(ticket.Id, ticket.EventId));

        return ticket;
    }

    private Ticket()
    {
    }

    internal Result MarkAsUsed()
    {
        UsedAtUtc = DateTime.UtcNow;

        RaiseEvent(new TicketUsedDomainEvent(Id));

        return Result.Success();
    }
}
