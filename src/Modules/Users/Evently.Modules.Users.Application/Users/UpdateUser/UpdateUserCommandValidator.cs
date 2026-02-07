using FluentValidation;

namespace Evently.Modules.Users.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("The user id is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("The first name is required.")
            .MaximumLength(200)
            .WithMessage("The first name must not exceed 200 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("The last name is required.")
            .MaximumLength(200)
            .WithMessage("The last name must not exceed 200 characters.");
    }
}
