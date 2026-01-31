using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Users.Application.Users.GetUser;
using Evently.Modules.Users.Domain.Users.DomainEvents;
using Evently.Modules.Users.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Users.Application.Users.RegisterUser;

internal sealed class UserRegisteredDomainEventHandler(ISender sender, IEventBus eventBus)
    : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        GetUserQuery query = new(notification.UserId);

        Result<UserResponse> result = await sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new EventlyException(nameof(GetUserQuery), result.Error);
        }

        UserRegisteredIntegrationEvent integrationEvent = new(
            notification.Id,
            notification.OccurredAtUtc,
            result.Value.Id,
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName);

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
