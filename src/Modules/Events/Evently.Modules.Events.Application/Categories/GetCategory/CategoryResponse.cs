namespace Evently.Modules.Events.Application.Categories.GetCategory;

public sealed record CategoryResponse
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required bool IsArchived { get; init; }
}
