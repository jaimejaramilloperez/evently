using Evently.Common.Application.Messaging;
using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Evently.Modules.Ticketing.Domain.Payments.DomainEvents;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPayment;

internal sealed class PaymentRefundedDomainEventHandler(IPaymentService paymentService)
    : IDomainEventHandler<PaymentRefundedDomainEvent>
{
    public async Task Handle(PaymentRefundedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        await paymentService.RefundAsync(domainEvent.TransactionId, domainEvent.RefundAmount);
    }
}
