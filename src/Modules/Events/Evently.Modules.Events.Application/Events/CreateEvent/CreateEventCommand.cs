using FluentValidation;
using MediatR;

namespace Evently.Modules.Events.Application.Events.CreateEvent;

public sealed record CreateEventCommand : IRequest<Guid>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Location { get; init; }
    public required DateTime StartsAtUtc { get; init; }
    public DateTime? EndsAtUtc { get; init; }
}

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    private static readonly TimeSpan ClockSkewBuffer = TimeSpan.FromMinutes(2);

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
            .Must(x => x >= timeProvider.GetUtcNow().Subtract(ClockSkewBuffer))
            .WithMessage("The start date cannot be in the past.");

        RuleFor(x => x.EndsAtUtc)
            .GreaterThan(x => x.StartsAtUtc)
            .WithMessage("The end date must be after the start date.")
            .When(x => x.EndsAtUtc.HasValue);
    }
}
