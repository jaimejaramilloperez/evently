namespace Evently.Modules.Users.Infrastructure.Identity;

public sealed record CredentialRepresentation
{
    public required string Type { get; init; } = string.Empty;
    public required string Value { get; init; } = string.Empty;
    public required bool Temporary { get; init; }
}
