using Evently.Common.Domain;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Payments;

public sealed class Payment : Entity
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid TransactionId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public decimal? AmountRefunded { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? RefundedAtUtc { get; }

    public static Guid CreatePaymentId()
    {
        return Guid.CreateVersion7();
    }

    public static Payment Create(Order order, Guid transactionId, decimal amount, string currency)
    {
        Payment payment = new()
        {
            Id = CreatePaymentId(),
            OrderId = order.Id,
            TransactionId = transactionId,
            Amount = amount,
            Currency = currency,
            CreatedAtUtc = DateTime.UtcNow,
        };

        payment.RaiseEvent(new PaymentCreatedDomainEvent(payment.Id));

        return payment;
    }

    private Payment()
    {
    }

    public Result Refund(decimal refundAmount)
    {
        if (AmountRefunded.HasValue && AmountRefunded == Amount)
        {
            return Result.Failure(PaymentErrors.AlreadyRefunded);
        }

        if (AmountRefunded + refundAmount > Amount)
        {
            return Result.Failure(PaymentErrors.NotEnoughFunds);
        }

        AmountRefunded += refundAmount;

        if (Amount == AmountRefunded)
        {
            RaiseEvent(new PaymentRefundedDomainEvent(Id, TransactionId, refundAmount));
        }
        else
        {
            RaiseEvent(new PaymentPartiallyRefundedDomainEvent(Id, TransactionId, refundAmount));
        }

        return Result.Success();
    }
}
