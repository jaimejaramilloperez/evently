using Evently.Common.Domain;
using Evently.Common.Domain.Results;
using Evently.Modules.Users.Domain.Users.DomainEvents;

namespace Evently.Modules.Users.Domain.Users;

public sealed class User : Entity
{
    public Guid Id { get; private set; }
    public Guid IdentityId { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    public static Guid CreateUserId()
    {
        return Guid.CreateVersion7();
    }

    public static User Create(string email, string firstName, string lastName, Guid identityId)
    {
        User user = new()
        {
            Id = CreateUserId(),
            IdentityId = identityId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };

        user.RaiseEvent(new UserRegisteredDomainEvent(user.Id));

        return user;
    }

    private User()
    {
    }

    public Result Update(string firstName, string lastName)
    {
        if (FirstName == firstName && LastName == lastName)
        {
            return Result.Success();
        }

        FirstName = firstName;
        LastName = lastName;

        RaiseEvent(new UserProfileUpdatedDomainEvent(Id, FirstName, LastName));

        return Result.Success();
    }
}
