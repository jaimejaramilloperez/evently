using FluentValidation;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class UpdateTicketTypePriceCommandValidator : AbstractValidator<UpdateTicketTypePriceCommand>
{
    public UpdateTicketTypePriceCommandValidator()
    {
        RuleFor(c => c.TicketTypeId)
            .NotEmpty()
            .WithMessage("The ticket id is required.");

        RuleFor(c => c.Price)
            .GreaterThan(decimal.Zero)
            .WithMessage("The price must be greater than zero.");
    }
}
