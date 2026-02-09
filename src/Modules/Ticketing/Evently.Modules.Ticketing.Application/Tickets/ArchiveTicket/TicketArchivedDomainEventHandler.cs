using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;
using Evently.Modules.Ticketing.IntegrationEvents;

namespace Evently.Modules.Ticketing.Application.Tickets.ArchiveTicket;

internal sealed class TicketArchivedDomainEventHandler(IEventBus eventBus)
    : IDomainEventHandler<TicketArchivedDomainEvent>
{
    public async Task Handle(TicketArchivedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        TicketArchivedIntegrationEvent integrationEvent = new(
                domainEvent.Id,
                domainEvent.OccurredAtUtc,
                domainEvent.TicketId,
                domainEvent.Code);

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
