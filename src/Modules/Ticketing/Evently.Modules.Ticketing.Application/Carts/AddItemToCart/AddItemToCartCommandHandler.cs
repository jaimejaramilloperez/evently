using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Tickets;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandHandler(
    CartService cartService,
    ICustomerRepository customerRepository,
    ITicketTypeRepository ticketTypeRepository)
    : ICommandHandler<AddItemToCartCommand>
{
    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        }

        TicketType? ticketType = await ticketTypeRepository.GetAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketErrors.NotFound(request.TicketTypeId));
        }

        if (ticketType.AvailableQuantity == 0)
        {
            return Result.Failure(TicketErrors.NotEnoughQuantity);
        }

        CartItem cartItem = new()
        {
            TicketTypeId = ticketType.Id,
            Quantity = request.Quantity,
            Price = ticketType.Price,
            Currency = ticketType.Currency,
        };

        await cartService.AddItemAsync(customer.Id, cartItem, cancellationToken);

        return Result.Success();
    }
}
