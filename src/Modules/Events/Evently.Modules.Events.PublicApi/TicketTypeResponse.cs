namespace Evently.Modules.Events.PublicApi;

public sealed record TicketTypeResponse
{
    public required Guid Id { get; init; }
    public required Guid EventId { get; init; }
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public required string Currency { get; init; }
    public required decimal Quantity { get; init; }
}

