using Evently.Modules.Ticketing.Application.Abstractions.Payments;

namespace Evently.Modules.Ticketing.Infrastructure.Payments;

internal sealed class PaymentService : IPaymentService
{
    public Task<PaymentResponse> ChargeAsync(decimal amount, string currency)
    {
        PaymentResponse response = new()
        {
            TransactionId = Guid.NewGuid(),
            Amount = amount,
            Currency = currency,
        };

        return Task.FromResult(response);
    }

    public Task RefundAsync(Guid transactionId, decimal amount)
    {
        return Task.CompletedTask;
    }
}
