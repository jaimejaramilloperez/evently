using Evently.Common.Domain.Results;
using Evently.IntegrationTests.Abstractions;
using Evently.Modules.Ticketing.Application.Carts.AddItemToCart;
using Evently.Modules.Ticketing.Application.Customers.GetCustomer;

namespace Evently.IntegrationTests.AddToCart;

public sealed class AddItemToCartTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private const decimal Quantity = 10;

    [Fact]
    public async Task Customer_ShouldBeAbleTo_AddItemToCart()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Result<Guid> userResult = await RegisterUserAsync(cancellationToken);
        Assert.True(userResult.IsSuccess);

        // Get customer
        Result<CustomerResponse> customerResult = await Poller.WaitAsync(
            TimeSpan.FromSeconds(15),
            async () =>
            {
                GetCustomerQuery query = new(userResult.Value);

                Result<CustomerResponse> customerResult = await SendAsync(query, cancellationToken);

                return customerResult;
            });

        Assert.True(customerResult.IsSuccess);

        // Act
        CustomerResponse customer = customerResult.Value;
        Guid ticketTypeId = Guid.NewGuid();

        await CreateEventAsync(Guid.NewGuid(), ticketTypeId, Quantity, cancellationToken);

        AddItemToCartCommand addItemToCartCommand = new()
        {
            CustomerId = customer.Id,
            TicketTypeId = ticketTypeId,
            Quantity = Quantity,
        };

        Result result = await SendAsync(addItemToCartCommand, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
