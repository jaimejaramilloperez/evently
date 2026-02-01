using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Payments.DomainEvents;

public sealed class PaymentPartiallyRefundedDomainEvent(Guid paymentId, Guid transactionId, decimal refundAmount)
    : DomainEvent
{
    public Guid PaymentId => paymentId;
    public Guid TransactionId => transactionId;
    public decimal RefundAmount => refundAmount;
}
