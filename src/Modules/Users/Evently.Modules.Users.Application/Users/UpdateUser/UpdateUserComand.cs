using Evently.Common.Application.Messaging;

namespace Evently.Modules.Users.Application.Users.UpdateUser;

public sealed record UpdateUserCommand : ICommand
{
    public required Guid UserId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
