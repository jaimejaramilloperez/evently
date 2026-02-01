using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Payments.DomainEvents;

public sealed class PaymentCreatedDomainEvent(Guid paymentId) : DomainEvent
{
    public Guid PaymentId => paymentId;
}
