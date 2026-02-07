using FluentValidation;

namespace Evently.Modules.Events.Application.Categories.UpdateCategory;

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("The category id is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("The name is required.")
            .MaximumLength(200)
            .WithMessage("The name must not exceed 200 characters.");
    }
}
