using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;
using Evently.Modules.Ticketing.IntegrationEvents;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;

internal sealed class EventPaymentsRefundedDomainEventHandler(IEventBus eventBus)
     : DomainEventHandler<EventPaymentsRefundedDomainEvent>
{
    public override async Task Handle(
        EventPaymentsRefundedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        EventPaymentsRefundedIntegrationEvent integrationEvent = new(
            domainEvent.EventId,
            domainEvent.OccurredAtUtc,
            domainEvent.EventId);

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
