using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Events.GetEvent;
using Evently.Modules.Events.Domain.Events.DomainEvents;
using Evently.Modules.Events.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Events.Application.Events.PublishEvent;

internal sealed class EventPublishedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<EventPublishedDomainEvent>
{
    public override async Task Handle(
        EventPublishedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        GetEventQuery query = new(domainEvent.EventId);

        Result<EventResponse?> result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(GetEventQuery), result.Error);
        }

        EventPublishedIntegrationEvent integrationEvent = new(
            domainEvent.Id,
            domainEvent.OccurredAtUtc,
            result.Value.Id,
            result.Value.Title,
            result.Value.Description,
            result.Value.Location,
            result.Value.StartsAtUtc,
            result.Value.EndsAtUtc,
            result.Value.TicketTypes.Select(x => new TicketTypeModel
            {
                Id = x.TicketTypeId,
                EventId = result.Value.Id,
                Name = x.Name,
                Price = x.Price,
                Currency = x.Currency,
                Quantity = x.Quantity,
            }).ToList());

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
