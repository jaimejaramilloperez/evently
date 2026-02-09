using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class CreateTicketsDomainEventHandler(ISender sender)
    : IDomainEventHandler<OrderCreatedDomainEvent>
{
    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        CreateTicketBatchCommand command = new(notification.OrderId);

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CreateTicketBatchCommand), result.Error);
        }
    }
}
