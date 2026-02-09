using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Events.CreateEvent;
using Evently.Modules.Events.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Events;

public sealed class EventPublishedIntegrationEventConsumer(ISender sender)
    : IConsumer<EventPublishedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<EventPublishedIntegrationEvent> context)
    {
        CreateEventCommand command = new()
        {
            EventId = context.Message.EventId,
            Title = context.Message.Title,
            Description = context.Message.Description,
            Location = context.Message.Location,
            StartsAtUtc = context.Message.StartsAtUtc,
            EndsAtUtc = context.Message.EndsAtUtc,
        };

        Result result = await sender.Send(command, context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CreateEventCommand), result.Error);
        }
    }
}
