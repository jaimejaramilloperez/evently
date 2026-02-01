using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Users.Domain.Users.DomainEvents;

public sealed class UserProfileUpdatedDomainEvent(Guid userId, string firstName, string lastName)
    : DomainEvent
{
    public Guid UserId => userId;
    public string FirstName => firstName;
    public string LastName => lastName;
}
