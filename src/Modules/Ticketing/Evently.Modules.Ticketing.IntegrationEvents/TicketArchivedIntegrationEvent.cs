using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class TicketArchivedIntegrationEvent : IntegrationEvent
{
    public Guid TicketId { get; init; }
    public string Code { get; init; }

    public TicketArchivedIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid ticketId,
        string code)
        : base(id, occurredAtUtc)
    {
        TicketId = ticketId;
        Code = code;
    }
}
