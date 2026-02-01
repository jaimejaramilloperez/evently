using Evently.Modules.Ticketing.Domain.Orders;

namespace Evently.Modules.Ticketing.Application.Orders.GetOrders;

public sealed record OrderResponse
{
    public required Guid Id { get; init; }
    public required Guid CustomerId { get; init; }
    public required OrderStatus Status { get; init; }
    public required decimal TotalPrice { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
}
