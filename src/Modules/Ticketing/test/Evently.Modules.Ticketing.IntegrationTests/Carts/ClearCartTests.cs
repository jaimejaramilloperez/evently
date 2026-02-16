using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Carts.ClearCart;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Modules.Ticketing.IntegrationTests.Carts;

public class ClearCartTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        ClearCartCommand command = new(Guid.CreateVersion7());

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(CustomerErrors.NotFound(command.CustomerId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenCustomerExists()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid customerId = await CreateCustomerAsync(Guid.CreateVersion7(), cancellationToken);

        ClearCartCommand command = new(customerId);

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
