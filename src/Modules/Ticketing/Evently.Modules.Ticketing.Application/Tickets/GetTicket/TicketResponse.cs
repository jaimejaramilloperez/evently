namespace Evently.Modules.Ticketing.Application.Tickets.GetTicket;

public sealed record TicketResponse
{
    public required Guid Id { get; init; }
    public required Guid CustomerId { get; init; }
    public required Guid OrderId { get; init; }
    public required Guid EventId { get; init; }
    public required Guid TicketTypeId { get; init; }
    public required string Code { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
}
