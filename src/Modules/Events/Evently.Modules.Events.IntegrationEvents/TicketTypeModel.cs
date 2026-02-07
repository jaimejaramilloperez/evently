namespace Evently.Modules.Events.IntegrationEvents;

public sealed class TicketTypeModel
{
    public Guid Id { get; init; }
    public Guid EventId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Currency { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
}
