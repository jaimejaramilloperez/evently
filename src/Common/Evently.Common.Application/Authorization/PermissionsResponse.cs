namespace Evently.Common.Application.Authorization;

public sealed record PermissionsResponse
{
    public required Guid UserId { get; init; }
    public required HashSet<string> Permissions { get; init; }
}
