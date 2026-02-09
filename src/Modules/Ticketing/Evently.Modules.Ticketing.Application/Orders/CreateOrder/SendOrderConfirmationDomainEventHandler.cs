using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Orders.GetOrder;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class SendOrderConfirmationDomainEventHandler(ISender sender)
    : IDomainEventHandler<OrderCreatedDomainEvent>
{
    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        GetOrderQuery query = new(notification.OrderId);

        Result<OrderResponse> result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(GetOrderQuery), result.Error);
        }

        // Send order confirmation notification.
    }
}
