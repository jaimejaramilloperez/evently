namespace Evently.Modules.Ticketing.Application.Abstractions.Payments;

public sealed record PaymentResponse
{
    public required Guid TransactionId { get; init; }
    public required decimal Amount { get; init; }
    public required string Currency { get; init; }
}
