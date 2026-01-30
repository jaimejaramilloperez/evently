using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

public sealed record AddItemToCartCommand : ICommand
{
    public required Guid CustomerId { get; init; }
    public required Guid TicketTypeId { get; init; }
    public required decimal Quantity { get; init; }
}
