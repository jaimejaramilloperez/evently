using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.Application.TicketTypes.UpdateTicketTypePrice;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.TicketTypes;

public sealed class TicketTypePriceChangedIntegrationEventConsumer(ISender sender)
    : IntegrationEventHandler<TicketTypePriceChangedIntegrationEvent>
{
    public override async Task Handle(
        TicketTypePriceChangedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        UpdateTicketTypePriceCommand command = new()
        {
            TicketTypeId = integrationEvent.TicketTypeId,
            Price = integrationEvent.Price,
        };

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(UpdateTicketTypePriceCommand), result.Error);
        }
    }
}
