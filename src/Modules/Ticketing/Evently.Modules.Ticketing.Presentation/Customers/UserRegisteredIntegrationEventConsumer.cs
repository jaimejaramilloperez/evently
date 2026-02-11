using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Customers.CreateCustomer;
using Evently.Modules.Users.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Customers;

public sealed class UserRegisteredIntegrationEventConsumer(ISender sender)
    : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(
        UserRegisteredIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        CreateCustomerCommand command = new()
        {
            CustomerId = integrationEvent.UserId,
            Email = integrationEvent.Email,
            FirstName = integrationEvent.FirstName,
            LastName = integrationEvent.LastName,
        };

        Result result = await sender.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            throw new EventlyException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}
