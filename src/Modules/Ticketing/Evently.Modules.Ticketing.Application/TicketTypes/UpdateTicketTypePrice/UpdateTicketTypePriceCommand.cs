using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.TicketTypes.UpdateTicketTypePrice;

public sealed record UpdateTicketTypePriceCommand : ICommand
{
    public Guid TicketTypeId { get; init; }
    public decimal Price { get; init; }
}
