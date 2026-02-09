using Evently.Common.Application.Messaging;
using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Evently.Modules.Ticketing.Domain.Payments.DomainEvents;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPayment;

internal sealed class PaymentPartiallyRefundedDomainEventHandler(IPaymentService paymentService)
    : IDomainEventHandler<PaymentPartiallyRefundedDomainEvent>
{
    public async Task Handle(PaymentPartiallyRefundedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await paymentService.RefundAsync(domainEvent.TransactionId, domainEvent.RefundAmount);
    }
}
