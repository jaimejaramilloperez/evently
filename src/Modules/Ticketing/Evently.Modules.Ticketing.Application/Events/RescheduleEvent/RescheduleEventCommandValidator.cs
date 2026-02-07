using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Events.RescheduleEvent;

internal sealed class RescheduleEventCommandValidator : AbstractValidator<RescheduleEventCommand>
{
    public RescheduleEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty();

        RuleFor(x => x.StartsAtUtc)
            .NotEmpty();

        RuleFor(x => x.EndsAtUtc)
            .Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc)
            .When(x => x.EndsAtUtc.HasValue);
    }
}
