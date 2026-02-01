using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Carts.RemoveItemFromCart;

internal sealed class RemoveItemFromCartCommandValidator : AbstractValidator<RemoveItemFromCartCommand>
{
    public RemoveItemFromCartCommandValidator()
    {
        RuleFor(c => c.CustomerId)
            .NotEmpty()
            .WithMessage("The customer id is required.");

        RuleFor(c => c.TicketTypeId)
            .NotEmpty()
            .WithMessage("The ticket type id is required.");
    }
}
