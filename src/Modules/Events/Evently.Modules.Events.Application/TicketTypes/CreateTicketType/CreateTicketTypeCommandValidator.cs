using FluentValidation;

namespace Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

internal sealed class CreateTicketTypeCommandValidator : AbstractValidator<CreateTicketTypeCommand>
{
    public CreateTicketTypeCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("The event id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name is required.")
            .MaximumLength(200)
            .WithMessage("The name must not exceed 200 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(decimal.Zero)
            .WithMessage("The price must be greater than zero.");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("The currency is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(decimal.Zero)
            .WithMessage("The quantity must be greater than zero.");

    }
}
