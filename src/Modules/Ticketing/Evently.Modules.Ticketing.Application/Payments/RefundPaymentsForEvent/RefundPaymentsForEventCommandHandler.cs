using System.Data.Common;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Payments;

namespace Evently.Modules.Ticketing.Application.Payments.RefundPaymentsForEvent;

internal sealed class RefundPaymentsForEventCommandHandler(
    IEventRepository eventRepository,
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RefundPaymentsForEventCommand>
{
    public async Task<Result> Handle(RefundPaymentsForEventCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteWithinStrategyAsync(async () =>
        {
            await using DbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);

            if (@event is null)
            {
                throw new EventlyException($"Event {request.EventId} not found");
            }

            IEnumerable<Payment> payments = await paymentRepository.GetForEventAsync(@event, cancellationToken);

            foreach (Payment payment in payments)
            {
                payment.Refund(payment.Amount - (payment.AmountRefunded ?? decimal.Zero));
            }

            @event.PaymentsRefunded();

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }, cancellationToken);

        return Result.Success();
    }
}
