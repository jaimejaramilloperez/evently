using Evently.Modules.Ticketing.Application.Customers.CreateCustomer;
using Evently.Modules.Ticketing.PublicApi;
using MediatR;

namespace Evently.Modules.Ticketing.Infrastructure.PublicApi;

internal sealed class TicketingApi(ISender sender) : ITicketingApi
{
    public async Task CreateCustomerAsync(
        Guid customerId,
        string email,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default)
    {
        CreateCustomerCommand command = new()
        {
            CustomerId = customerId,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };

        await sender.Send(command, cancellationToken);
    }
}
