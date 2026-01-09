namespace Evently.Modules.Events.Application.Events.GetEvent;

public sealed record TicketTypeResponse
{
    public required Guid TicketTypeId { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public required string Currency { get; init; }
    public required decimal Quantity { get; init; }
}
