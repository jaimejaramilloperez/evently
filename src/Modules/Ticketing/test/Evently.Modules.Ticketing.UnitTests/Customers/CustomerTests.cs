using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Modules.Ticketing.UnitTests.Customers;

public class CustomerTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnValue_WhenCustomerIsCreated()
    {
        // Act
        Result<Customer> customer = Customer.Create(
            Guid.CreateVersion7(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Assert
        Assert.NotNull(customer.Value);
    }
}
