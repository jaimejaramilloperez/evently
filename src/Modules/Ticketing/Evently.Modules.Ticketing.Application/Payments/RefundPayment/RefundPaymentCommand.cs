using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPayment;

public sealed record RefundPaymentCommand : ICommand
{
    public required Guid PaymentId { get; init; }
    public required decimal Amount { get; init; }
}
