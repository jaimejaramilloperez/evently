using FluentValidation;

namespace Evently.Modules.Attendance.Application.Events.CreateEvent;

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.Location)
            .NotEmpty();

        RuleFor(x => x.StartsAtUtc)
            .NotEmpty();

        RuleFor(x => x.EndsAtUtc)
            .Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc).When(x => x.EndsAtUtc.HasValue);
    }
}
