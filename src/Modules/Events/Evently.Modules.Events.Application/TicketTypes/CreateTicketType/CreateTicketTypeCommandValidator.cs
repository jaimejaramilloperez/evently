using FluentValidation;

namespace Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

internal sealed class CreateTicketTypeCommandValidator : AbstractValidator<CreateTicketTypeCommand>
{
    public CreateTicketTypeCommandValidator()
    {
        RuleFor(c => c.EventId)
            .NotEmpty()
            .WithMessage("The event id is required.");

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("The name is required.")
            .MaximumLength(200)
            .WithMessage("The name must not exceed 200 characters.");

        RuleFor(c => c.Price)
            .GreaterThan(decimal.Zero)
            .WithMessage("The price must be greater than zero.");

        RuleFor(c => c.Currency)
            .NotEmpty()
            .WithMessage("The currency is required.");

        RuleFor(c => c.Quantity)
            .GreaterThan(decimal.Zero)
            .WithMessage("The quantity must be greater than zero.");

    }
}
