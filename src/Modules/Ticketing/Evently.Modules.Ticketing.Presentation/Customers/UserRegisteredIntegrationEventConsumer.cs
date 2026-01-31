using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Customers.CreateCustomer;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Evently.Modules.Ticketing.Presentation.Customers;

public sealed class UserRegisteredIntegrationEventConsumer(ISender sender) : IConsumer<UserRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        CreateCustomerCommand command = new()
        {
            CustomerId = context.Message.UserId,
            Email = context.Message.Email,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName,
        };

        Result result = await sender.Send(command, context.CancellationToken);

        if (!result.IsSuccess)
        {
            throw new EventlyException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}
