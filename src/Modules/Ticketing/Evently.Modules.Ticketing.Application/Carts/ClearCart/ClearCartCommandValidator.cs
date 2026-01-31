using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Carts.ClearCart;

public sealed class ClearCartCommandValidator : AbstractValidator<ClearCartCommand>
{
    public ClearCartCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("The customer id is required.");
    }
}
