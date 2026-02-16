using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.Application.Events.CreateEvent;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Events;

public sealed class EventPublishedIntegrationEventConsumer(ISender sender)
    : IntegrationEventHandler<EventPublishedIntegrationEvent>
{
    public override async Task Handle(
        EventPublishedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        CreateEventCommand command = new()
        {
            EventId = integrationEvent.EventId,
            Title = integrationEvent.Title,
            Description = integrationEvent.Description,
            Location = integrationEvent.Location,
            StartsAtUtc = integrationEvent.StartsAtUtc,
            EndsAtUtc = integrationEvent.EndsAtUtc,
            TicketTypes = integrationEvent.TicketTypes
                .Select(x => new CreateEventCommand.TicketTypeRequest()
                {
                    TicketTypeId = x.Id,
                    EventId = integrationEvent.EventId,
                    Name = x.Name,
                    Price = x.Price,
                    Currency = x.Currency,
                    Quantity = x.Quantity,
                })
                .ToList(),
        };

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CreateEventCommand), result.Error);
        }
    }
}
