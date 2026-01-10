using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;

namespace Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

public sealed record CreateTicketTypeCommand : ICommand<TicketTypeResponse>
{
    public required Guid EventId { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public required string Currency { get; init; }
    public required decimal Quantity { get; init; }
}
