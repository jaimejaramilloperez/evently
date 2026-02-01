using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Carts.RemoveItemFromCart;

public sealed record RemoveItemFromCartCommand : ICommand
{
    public required Guid CustomerId { get; init; }
    public required Guid TicketTypeId { get; init; }
}
