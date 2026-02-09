using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Orders.GetOrder;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class OrderCreatedDomainEventHandler(ISender sender, IEventBus eventBus)
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

        OrderCreatedIntegrationEvent integrationEvent = new(
                notification.Id,
                notification.OccurredAtUtc,
                result.Value.Id,
                result.Value.CustomerId,
                result.Value.TotalPrice,
                result.Value.CreatedAtUtc,
                result.Value.OrderItems.Select(x => new OrderItemModel
                {
                    Id = x.OrderItemId,
                    OrderId = result.Value.Id,
                    TicketTypeId = x.TicketTypeId,
                    Price = x.Price,
                    UnitPrice = x.UnitPrice,
                    Currency = x.Currency,
                    Quantity = x.Quantity
                }).ToList());

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
