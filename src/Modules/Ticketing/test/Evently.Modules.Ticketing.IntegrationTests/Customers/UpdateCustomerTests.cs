using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Customers.UpdateCustomer;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Modules.Ticketing.IntegrationTests.Customers;

public class UpdateCustomerTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        UpdateCustomerCommand command = new()
        {
            CustomerId = Guid.CreateVersion7(),
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(CustomerErrors.NotFound(command.CustomerId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCustomerIsUpdated()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid customerId = await CreateCustomerAsync(Guid.CreateVersion7(), cancellationToken);

        UpdateCustomerCommand command = new()
        {
            CustomerId = customerId,
            FirstName = Faker.Name.FirstName(),
            LastName = Faker.Name.LastName(),
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
