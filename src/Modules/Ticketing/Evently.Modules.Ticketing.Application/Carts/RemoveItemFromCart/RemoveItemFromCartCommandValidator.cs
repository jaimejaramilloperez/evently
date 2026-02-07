using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Carts.RemoveItemFromCart;

internal sealed class RemoveItemFromCartCommandValidator : AbstractValidator<RemoveItemFromCartCommand>
{
    public RemoveItemFromCartCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("The customer id is required.");

        RuleFor(x => x.TicketTypeId)
            .NotEmpty()
            .WithMessage("The ticket type id is required.");
    }
}
