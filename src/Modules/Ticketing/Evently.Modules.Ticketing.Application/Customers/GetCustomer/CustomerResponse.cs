namespace Evently.Modules.Ticketing.Application.Customers.GetCustomer;

public sealed record CustomerResponse
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
};
