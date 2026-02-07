using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.IntegrationEvents;

public sealed class OrderCreatedIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; init; }
    public Guid CustomerId { get; init; }
    public decimal TotalPrice { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public List<OrderItemModel> OrderItems { get; init; }

    public OrderCreatedIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid orderId,
        Guid customerId,
        decimal totalPrice,
        DateTime createdAtUtc,
        List<OrderItemModel> orderItems)
        : base(id, occurredAtUtc)
    {
        OrderId = orderId;
        CustomerId = customerId;
        TotalPrice = totalPrice;
        CreatedAtUtc = createdAtUtc;
        OrderItems = orderItems;
    }
}
