using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Domain.TicketTypes.DomainEvents;
using Evently.Modules.Events.IntegrationEvents;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class TicketTypePriceChangedDomainEventHandler(IEventBus eventBus)
    : IDomainEventHandler<TicketTypePriceChangedDomainEvent>
{
    public async Task Handle(TicketTypePriceChangedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        TicketTypePriceChangedIntegrationEvent integrationEvent = new(
                domainEvent.Id,
                domainEvent.OccurredAtUtc,
                domainEvent.TicketTypeId,
                domainEvent.Price);

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
