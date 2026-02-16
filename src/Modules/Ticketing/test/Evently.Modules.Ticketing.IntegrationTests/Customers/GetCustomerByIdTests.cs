using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Customers.GetCustomer;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Modules.Ticketing.IntegrationTests.Customers;

public class GetCustomerByIdTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        GetCustomerQuery query = new(Guid.CreateVersion7());

        // Act
        Result result = await SendAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(CustomerErrors.NotFound(query.CustomerId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnCustomer_WhenCustomerExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid customerId = await CreateCustomerAsync(Guid.CreateVersion7(), cancellationToken);

        GetCustomerQuery query = new(customerId);

        // Act
        Result<CustomerResponse> result = await SendAsync(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}
