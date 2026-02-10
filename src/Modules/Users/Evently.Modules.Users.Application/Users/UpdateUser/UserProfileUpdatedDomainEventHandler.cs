using Evently.Common.Application.EventBus;
using Evently.Common.Application.Messaging;
using Evently.Modules.Users.Domain.Users.DomainEvents;
using Evently.Modules.Users.IntegrationEvents;

namespace Evently.Modules.Users.Application.Users.UpdateUser;

internal sealed class UserProfileUpdatedDomainEventHandler(IEventBus eventBus)
    : DomainEventHandler<UserProfileUpdatedDomainEvent>
{
    public override async Task Handle(
        UserProfileUpdatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        UserProfileUpdatedIntegrationEvent integrationEvent = new(
            domainEvent.Id,
            domainEvent.OccurredAtUtc,
            domainEvent.UserId,
            domainEvent.FirstName,
            domainEvent.LastName);

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
