using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;
using Evently.Modules.Ticketing.IntegrationEvents;

namespace Evently.Modules.Ticketing.Application.Tickets.ArchiveTicketsForEvent;

internal sealed class EventTicketsArchivedDomainEventHandler(IEventBus eventBus)
     : DomainEventHandler<EventTicketsArchivedDomainEvent>
{
    public override async Task Handle(
        EventTicketsArchivedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        EventTicketsArchivedIntegrationEvent integrationEvent = new(
            domainEvent.EventId,
            domainEvent.OccurredAtUtc,
            domainEvent.EventId);

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
