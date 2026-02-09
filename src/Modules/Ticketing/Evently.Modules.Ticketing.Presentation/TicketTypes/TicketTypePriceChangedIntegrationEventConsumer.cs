using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.Application.TicketTypes.UpdateTicketTypePrice;
using MassTransit;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.TicketTypes;

public sealed class TicketTypePriceChangedIntegrationEventConsumer(ISender sender)
    : IConsumer<TicketTypePriceChangedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<TicketTypePriceChangedIntegrationEvent> context)
    {
        UpdateTicketTypePriceCommand command = new()
        {
            TicketTypeId = context.Message.TicketTypeId,
            Price = context.Message.Price,
        };

        Result result = await sender.Send(command, context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(UpdateTicketTypePriceCommand), result.Error);
        }
    }
}
