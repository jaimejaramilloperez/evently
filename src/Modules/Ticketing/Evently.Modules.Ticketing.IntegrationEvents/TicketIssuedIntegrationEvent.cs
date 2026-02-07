using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class TicketIssuedIntegrationEvent : IntegrationEvent
{
    public Guid TicketId { get; init; }
    public Guid CustomerId { get; init; }
    public Guid EventId { get; init; }
    public string Code { get; init; }

    public TicketIssuedIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid ticketId,
        Guid customerId,
        Guid eventId,
        string code)
        : base(id, occurredAtUtc)
    {
        TicketId = ticketId;
        CustomerId = customerId;
        EventId = eventId;
        Code = code;
    }
}
