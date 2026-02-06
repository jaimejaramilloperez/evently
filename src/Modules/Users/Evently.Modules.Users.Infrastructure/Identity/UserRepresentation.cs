namespace Evently.Modules.Users.Infrastructure.Identity;

public sealed record UserRepresentation
{
    public required string Username { get; init; } = string.Empty;
    public required string Email { get; init; } = string.Empty;
    public required string FirstName { get; init; } = string.Empty;
    public required string LastName { get; init; } = string.Empty;
    public required bool EmailVerified { get; init; }
    public required bool Enabled { get; init; }
    public required ICollection<CredentialRepresentation> Credentials { get; init; } = [];
}
