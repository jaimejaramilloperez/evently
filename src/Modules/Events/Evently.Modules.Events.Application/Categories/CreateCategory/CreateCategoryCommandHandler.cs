using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Categories.GetCategory;
using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Application.Categories.CreateCategory;

internal sealed class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCategoryCommand, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = Category.Create(request.Name);

        categoryRepository.Insert(category);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CategoryResponse()
        {
            Id = category.Id,
            Name = category.Name,
            IsArchived = category.IsArchived,
        };
    }
}
