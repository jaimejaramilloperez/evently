using Evently.Common.Domain.Results;

namespace Evently.Modules.Events.Domain.Categories;

public static class CategoryErrors
{
    public static Error NotFound(Guid categoryId) => Error.NotFound(
        "Categories.NotFound",
        $"The category with the identifier {categoryId} was not found");

    public static readonly Error NameEmpty = Error.Problem(
        "Categories.NameEmpty",
        "The category name can not be empty");

    public static readonly Error AlreadyArchived = Error.Problem(
        "Categories.AlreadyArchived",
        "The category was already archived");
}
