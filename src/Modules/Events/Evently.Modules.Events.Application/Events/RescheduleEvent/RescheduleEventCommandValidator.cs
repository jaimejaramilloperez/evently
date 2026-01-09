using Evently.Modules.Events.Domain.Events;
using FluentValidation;

namespace Evently.Modules.Events.Application.Events.RescheduleEvent;

internal sealed class RescheduleEventCommandValidator : AbstractValidator<RescheduleEventCommand>
{
    public RescheduleEventCommandValidator(TimeProvider timeProvider)
    {
        RuleFor(c => c.EventId)
            .NotEmpty()
            .WithMessage("The event id is required.");

        RuleFor(x => x.StartsAtUtc)
            .NotEmpty()
            .WithMessage("The start date is required.")
            .Must(x => x >= timeProvider.GetUtcNow().UtcDateTime.Subtract(EventConstants.AllowedClockSkew))
            .WithMessage("The start date cannot be in the past.");

        RuleFor(x => x.EndsAtUtc)
            .GreaterThan(x => x.StartsAtUtc)
            .WithMessage("The end date must be after the start date.")
            .When(x => x.EndsAtUtc.HasValue);
    }
}
