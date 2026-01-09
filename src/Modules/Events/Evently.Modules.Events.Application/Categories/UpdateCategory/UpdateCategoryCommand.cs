using Evently.Modules.Events.Application.Abstractions.Messaging;

namespace Evently.Modules.Events.Application.Categories.UpdateCategory;

public sealed record UpdateCategoryCommand : ICommand
{
    public required Guid CategoryId { get; init; }
    public required string Name { get; init; }
}
