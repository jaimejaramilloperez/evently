namespace Evently.Modules.Users.Application.Abstractions.Identity;

public sealed record UserModel
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
