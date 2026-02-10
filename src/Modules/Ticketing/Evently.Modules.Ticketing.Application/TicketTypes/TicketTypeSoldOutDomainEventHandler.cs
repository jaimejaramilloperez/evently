using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;
using Evently.Modules.Ticketing.IntegrationEvents;

namespace Evently.Modules.Ticketing.Application.TicketTypes;

internal sealed class TicketTypeSoldOutDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<TicketTypeSoldOutDomainEvent>
{
    public override async Task Handle(
        TicketTypeSoldOutDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        TicketTypeSoldOutIntegrationEvent integrationEvent = new(
            domainEvent.Id,
            domainEvent.OccurredAtUtc,
            domainEvent.TicketTypeId);

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
