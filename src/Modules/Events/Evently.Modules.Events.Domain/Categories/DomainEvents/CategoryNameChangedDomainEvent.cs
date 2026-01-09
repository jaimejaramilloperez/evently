using Evently.Modules.Events.Domain.Abstractions.DomainEvents;

namespace Evently.Modules.Events.Domain.Categories.DomainEvents;

public sealed class CategoryNameChangedDomainEvent(Guid categoryId, string name)
    : DomainEvent
{
    public Guid CategoryId { get; init; } = categoryId;
    public string Name { get; init; } = name;
}
