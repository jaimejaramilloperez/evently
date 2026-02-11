using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Customers.UpdateCustomer;
using Evently.Modules.Users.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Customers;

public sealed class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IntegrationEventHandler<UserProfileUpdatedIntegrationEvent>
{
    public override async Task Handle(
        UserProfileUpdatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        UpdateCustomerCommand command = new()
        {
            CustomerId = integrationEvent.UserId,
            FirstName = integrationEvent.FirstName,
            LastName = integrationEvent.LastName,
        };

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(UpdateCustomerCommand), result.Error);
        }
    }
}
