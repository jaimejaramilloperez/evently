using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Application.Orders.CreateOrder;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Modules.Ticketing.IntegrationTests.Orders;

public class CreateOrderTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        CreateOrderCommand command = new(Guid.CreateVersion7());

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(CustomerErrors.NotFound(command.CustomerId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenCartIsEmpty()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid customerId = await CreateCustomerAsync(Guid.CreateVersion7(), cancellationToken);

        CreateOrderCommand command = new(customerId);

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(CartErrors.Empty, result.Error);
    }
}
