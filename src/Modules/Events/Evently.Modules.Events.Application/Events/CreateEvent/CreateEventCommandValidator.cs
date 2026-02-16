using Evently.Modules.Events.Domain.Events;
using FluentValidation;

namespace Evently.Modules.Events.Application.Events.CreateEvent;

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("The title is required.")
            .MaximumLength(200)
            .WithMessage("The title must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("The description is required.")
            .MaximumLength(2000)
            .WithMessage("The description must not exceed 2000 characters.");

        RuleFor(x => x.Location)
            .NotEmpty()
            .WithMessage("The location is required.")
            .MaximumLength(500)
            .WithMessage("The location must not exceed 500 characters.");

        RuleFor(x => x.StartsAtUtc)
            .NotEmpty()
            .WithMessage("The start date is required.")
            .Must(x => x >= timeProvider.GetUtcNow().Subtract(EventConstants.AllowedClockSkew).UtcDateTime)
            .WithErrorCode(EventErrors.StartDateInPast.Code)
            .WithMessage("The start date cannot be in the past.");

        RuleFor(x => x.EndsAtUtc)
            .GreaterThan(x => x.StartsAtUtc)
            .WithMessage("The end date must be after the start date.")
            .When(x => x.EndsAtUtc.HasValue);
    }
}
