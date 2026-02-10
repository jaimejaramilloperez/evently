using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Domain.Events.DomainEvents;
using Evently.Modules.Events.IntegrationEvents;

namespace Evently.Modules.Events.Application.Events.RescheduleEvent;

internal sealed class EventRescheduledDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<EventRescheduledDomainEvent>
{
    public override async Task Handle(
        EventRescheduledDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        EventRescheduledIntegrationEvent integrationEvent = new(
            domainEvent.Id,
            domainEvent.OccurredAtUtc,
            domainEvent.EventId,
            domainEvent.StartsAtUtc,
            domainEvent.EndsAtUtc);

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
