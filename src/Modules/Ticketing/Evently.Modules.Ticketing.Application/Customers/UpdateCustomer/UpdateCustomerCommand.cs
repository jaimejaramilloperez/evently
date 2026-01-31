using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Customers.UpdateCustomer;

public sealed record UpdateCustomerCommand : ICommand
{
    public required Guid CustomerId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
