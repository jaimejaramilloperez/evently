using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Customers.UpdateCustomer;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Customers;

public sealed class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IConsumer<UserProfileUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserProfileUpdatedIntegrationEvent> context)
    {
        UpdateCustomerCommand command = new()
        {
            CustomerId = context.Message.UserId,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName,
        };

        Result result = await sender.Send(command, context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(UpdateCustomerCommand), result.Error);
        }
    }
}
