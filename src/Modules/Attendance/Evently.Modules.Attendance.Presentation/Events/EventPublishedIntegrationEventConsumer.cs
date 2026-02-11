using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Events.CreateEvent;
using Evently.Modules.Events.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Events;

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
        };

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CreateEventCommand), result.Error);
        }
    }
}
