using Evently.Common.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

public sealed record RegisterUserCommand : ICommand<Guid>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
