using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Customers.CreateCustomer;

public sealed record CreateCustomerCommand : ICommand
{
    public required Guid CustomerId { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}
