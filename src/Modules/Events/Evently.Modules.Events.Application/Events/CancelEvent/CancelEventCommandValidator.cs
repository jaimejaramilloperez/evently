using FluentValidation;

namespace Evently.Modules.Events.Application.Events.CancelEvent;

internal sealed class CancelEventCommandValidator : AbstractValidator<CancelEventCommand>
{
    public CancelEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty()
            .WithMessage("The event id is required.");
    }
}
