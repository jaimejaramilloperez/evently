using FluentValidation;

namespace Evently.Modules.Ticketing.Application.Customers.CreateCustomer;

internal sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("The customer id is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("The email is required.")
            .EmailAddress()
            .WithMessage("The email must be a valid email address.")
            .MaximumLength(300)
            .WithMessage("The email must not exceed 300 characters.");

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
