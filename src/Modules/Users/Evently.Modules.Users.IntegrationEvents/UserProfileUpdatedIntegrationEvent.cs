using Evently.Common.Application.EventBus;

namespace Evently.Modules.Users.IntegrationEvents;

public sealed class UserProfileUpdatedIntegrationEvent : IntegrationEvent
{
    public Guid UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }

    public UserProfileUpdatedIntegrationEvent(
        Guid id,
        DateTime occurredAtUtc,
        Guid userId,
        string firstName,
        string lastName)
        : base(id, occurredAtUtc)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
    }
}
