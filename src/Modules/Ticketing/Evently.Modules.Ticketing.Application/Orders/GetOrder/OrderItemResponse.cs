namespace Evently.Modules.Ticketing.Application.Orders.GetOrder;

public sealed record OrderItemResponse
{
    public required Guid OrderItemId { get; init; }
    public required Guid OrderId { get; init; }
    public required Guid TicketTypeId { get; init; }
    public required decimal Quantity { get; init; }
    public required decimal UnitPrice { get; init; }
    public required decimal Price { get; init; }
    public required string Currency { get; init; }
}
