using Evently.Modules.Events.Domain.Abstractions.DomainEvents;

namespace Evently.Modules.Events.Domain.Categories.DomainEvents;

public sealed class CategoryArchivedDomainEvent(Guid categoryId) : DomainEvent
{
    public Guid CategoryId { get; init; } = categoryId;
}
