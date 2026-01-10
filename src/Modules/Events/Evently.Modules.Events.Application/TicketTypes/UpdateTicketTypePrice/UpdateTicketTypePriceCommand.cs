using Evently.Common.Application.Messaging;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

public sealed record UpdateTicketTypePriceCommand : ICommand
{
    public required Guid TicketTypeId { get; init; }
    public required decimal Price { get; init; }
}
