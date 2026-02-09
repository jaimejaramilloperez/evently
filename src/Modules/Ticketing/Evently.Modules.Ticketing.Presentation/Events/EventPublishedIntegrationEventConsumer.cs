using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.Application.Events.CreateEvent;
using MassTransit;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Events;

public sealed class EventPublishedIntegrationEventConsumer(ISender sender)
    : IConsumer<EventPublishedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<EventPublishedIntegrationEvent> context)
    {
        List<CreateEventCommand.TicketTypeRequest> ticketTypes = context.Message.TicketTypes
            .Select(t => new CreateEventCommand.TicketTypeRequest(
                t.Id,
                context.Message.EventId,
                t.Name,
                t.Price,
                t.Currency,
                t.Quantity))
            .ToList();

        CreateEventCommand command = new()
        {
            EventId = context.Message.EventId,
            Title = context.Message.Title,
            Description = context.Message.Description,
            Location = context.Message.Location,
            StartsAtUtc = context.Message.StartsAtUtc,
            EndsAtUtc = context.Message.EndsAtUtc,
            TicketTypes = ticketTypes,
        };

        Result result = await sender.Send(command, context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CreateEventCommand), result.Error);
        }
    }
}
