using FluentValidation;

namespace Evently.Modules.Events.Application.Categories.CreateCategory;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("The name is required.")
            .MaximumLength(200)
            .WithMessage("The name must not exceed 200 characters.");
    }
}
