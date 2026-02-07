using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
{
    public AddItemToCartCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("The customer id is required.");

        RuleFor(x => x.TicketTypeId)
            .NotEmpty()
            .WithMessage("The ticket type id is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(decimal.Zero)
            .WithMessage("The quantity must be greater than zero.");
    }
}
