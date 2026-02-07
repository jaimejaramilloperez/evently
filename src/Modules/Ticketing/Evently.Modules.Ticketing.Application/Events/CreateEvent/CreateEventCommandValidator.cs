using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Events.CreateEvent;

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
            .Must((cmd, endsAt) => endsAt > cmd.StartsAtUtc)
            .When(x => x.EndsAtUtc.HasValue);

        RuleForEach(x => x.TicketTypes)
            .ChildRules(t =>
            {
                t.RuleFor(r => r.EventId).NotEmpty();
                t.RuleFor(r => r.Name).NotEmpty();
                t.RuleFor(r => r.Price).GreaterThan(decimal.Zero);
                t.RuleFor(r => r.Currency).NotEmpty();
                t.RuleFor(r => r.Quantity).GreaterThan(decimal.Zero);
            });
    }
}
