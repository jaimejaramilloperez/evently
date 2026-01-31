using Evently.Common.Domain;
using Evently.Common.Domain.Results;

namespace Evently.Modules.Ticketing.Domain.Customers;

public sealed class Customer : Entity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    public static Customer Create(Guid id, string email, string firstName, string lastName)
    {
        Customer user = new()
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };

        return user;
    }

    private Customer()
    {
    }

    public Result Update(string firstName, string lastName)
    {
        if (FirstName == firstName && LastName == lastName)
        {
            return Result.Success();
        }

        FirstName = firstName;
        LastName = lastName;

        return Result.Success();
    }
}
