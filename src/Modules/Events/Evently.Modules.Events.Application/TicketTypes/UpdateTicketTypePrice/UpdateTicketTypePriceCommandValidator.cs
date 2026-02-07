using FluentValidation;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class UpdateTicketTypePriceCommandValidator : AbstractValidator<UpdateTicketTypePriceCommand>
{
    public UpdateTicketTypePriceCommandValidator()
    {
        RuleFor(x => x.TicketTypeId)
            .NotEmpty()
            .WithMessage("The ticket id is required.");

        RuleFor(x => x.Price)
            .GreaterThan(decimal.Zero)
            .WithMessage("The price must be greater than zero.");
    }
}
