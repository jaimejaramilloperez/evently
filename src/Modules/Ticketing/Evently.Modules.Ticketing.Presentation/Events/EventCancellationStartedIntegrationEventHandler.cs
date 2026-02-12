using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.Application.Events.CancelEvent;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Events;

internal sealed class EventCancellationStartedIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<EventCancellationStartedIntegrationEvent>
{
    public override async Task Handle(
        EventCancellationStartedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        CancelEventCommand command = new(integrationEvent.EventId);

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CancelEventCommand), result.Error);
        }
    }
}
