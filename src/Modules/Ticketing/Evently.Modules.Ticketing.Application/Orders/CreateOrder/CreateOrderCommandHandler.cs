using System.Data.Common;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Application.Abstractions.Payments;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;

namespace Evently.Modules.Ticketing.Application.Orders.CreateOrder;

internal sealed class CreateOrderCommandHandler(
    ICustomerRepository customerRepository,
    IOrderRepository orderRepository,
    ITicketTypeRepository ticketTypeRepository,
    IPaymentRepository paymentRepository,
    IPaymentService paymentService,
    CartService cartService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateOrderCommand>
{
    public async Task<Result> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.ExecuteWithinStrategyAsync(async () =>
        {
            await using DbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            Customer? customer = await customerRepository.GetAsync(request.CustomerId, cancellationToken);

            if (customer is null)
            {
                throw new EventlyException(CustomerErrors.NotFound(request.CustomerId).Description);
            }

            Order order = Order.Create(customer);

            Cart cart = await cartService.GetAsync(customer.Id, cancellationToken);

            if (cart.Items.Count == 0)
            {
                throw new EventlyException(CartErrors.Empty.Description);
            }

            foreach (CartItem cartItem in cart.Items)
            {
                // This acquires a pessimistic lock or throws an exception if already locked.
                TicketType? ticketType = await ticketTypeRepository.GetWithLockAsync(
                    cartItem.TicketTypeId,
                    cancellationToken);

                if (ticketType is null)
                {
                    throw new EventlyException(TicketTypeErrors.NotFound(cartItem.TicketTypeId).Description);
                }

                Result result = ticketType.UpdateQuantity(cartItem.Quantity);

                if (result.IsFailure)
                {
                    throw new EventlyException(result.Error.Description);
                }

                order.AddItem(ticketType, cartItem.Quantity, cartItem.Price, ticketType.Currency);
            }

            orderRepository.Insert(order);

            // We're faking a payment gateway request here...
            PaymentResponse paymentResponse = await paymentService.ChargeAsync(order.TotalPrice, order.Currency);

            Payment payment = Payment.Create(
                order,
                paymentResponse.TransactionId,
                paymentResponse.Amount,
                paymentResponse.Currency);

            paymentRepository.Insert(payment);

            await unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            await cartService.ClearAsync(customer.Id, cancellationToken);
        }, cancellationToken);

        return Result.Success();
    }
}
