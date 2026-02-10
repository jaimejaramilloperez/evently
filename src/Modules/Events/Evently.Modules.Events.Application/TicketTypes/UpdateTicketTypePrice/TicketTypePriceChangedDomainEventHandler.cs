using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Domain.TicketTypes.DomainEvents;
using Evently.Modules.Events.IntegrationEvents;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class TicketTypePriceChangedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<TicketTypePriceChangedDomainEvent>
{
    public override async Task Handle(
        TicketTypePriceChangedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        TicketTypePriceChangedIntegrationEvent integrationEvent = new(
            domainEvent.Id,
            domainEvent.OccurredAtUtc,
            domainEvent.TicketTypeId,
            domainEvent.Price);

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
