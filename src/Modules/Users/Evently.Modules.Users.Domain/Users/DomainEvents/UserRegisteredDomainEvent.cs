using Evently.Common.Domain.DomainEvents;

namespace Evently.Modules.Users.Domain.Users.DomainEvents;

public sealed class UserRegisteredDomainEvent(Guid userId) : DomainEvent
{
    public Guid UserId { get; init; } = userId;
}
