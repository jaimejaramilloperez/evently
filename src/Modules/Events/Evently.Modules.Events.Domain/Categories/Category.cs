using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Abstractions.Results;
using Evently.Modules.Events.Domain.Categories.DomainEvents;

namespace Evently.Modules.Events.Domain.Categories;

public sealed class Category : Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsArchived { get; private set; }

    public static Guid CreateCategoryId()
    {
        return Guid.CreateVersion7();
    }

    public static Category Create(string name)
    {
        Category category = new()
        {
            Id = CreateCategoryId(),
            Name = name,
            IsArchived = false,
        };

        category.RaiseEvent(new CategoryCreatedDomainEvent(category.Id));

        return category;
    }

    private Category()
    {
    }

    public Result ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(CategoryErrors.NameEmpty);
        }

        if (Name == name)
        {
            return Result.Success();
        }

        Name = name;

        RaiseEvent(new CategoryNameChangedDomainEvent(Id, Name));

        return Result.Success();
    }

    public Result Archive()
    {
        if (IsArchived)
        {
            return Result.Failure(CategoryErrors.AlreadyArchived);
        }

        IsArchived = true;

        RaiseEvent(new CategoryArchivedDomainEvent(Id));

        return Result.Success();
    }
}
