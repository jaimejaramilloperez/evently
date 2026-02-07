using FluentValidation;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
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

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("The email is required.")
            .EmailAddress()
            .WithMessage("The email must be a valid email address.")
            .MaximumLength(300)
            .WithMessage("The email must not exceed 300 characters.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("The password is required.")
            .MaximumLength(200)
            .WithMessage("The password must not exceed 200 characters.");
    }
}
