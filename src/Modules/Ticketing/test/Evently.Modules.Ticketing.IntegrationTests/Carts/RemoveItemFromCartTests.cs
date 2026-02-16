using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Carts.RemoveItemFromCart;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Modules.Ticketing.IntegrationTests.Carts;

public class RemoveItemFromCartTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private const decimal Quantity = 10;

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        RemoveItemFromCartCommand command = new()
        {
            CustomerId = Guid.CreateVersion7(),
            TicketTypeId = Guid.CreateVersion7(),
        };

        // Act
        Result result = await SendAsync(command, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equivalent(CustomerErrors.NotFound(command.CustomerId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenTicketTypeDoesNotExist()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        Guid customerId = await CreateCustomerAsync(Guid.CreateVersion7(), cancellationToken);

        RemoveItemFromCartCommand command = new()
        {
            CustomerId = customerId,
            TicketTypeId = Guid.CreateVersion7(),
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(TicketTypeErrors.NotFound(command.TicketTypeId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenRemovedItemFromCart()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid customerId = await CreateCustomerAsync(Guid.CreateVersion7(), cancellationToken);
        var eventId = Guid.CreateVersion7();
        var ticketTypeId = Guid.CreateVersion7();

        await CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity, cancellationToken);

        RemoveItemFromCartCommand command = new()
        {
            CustomerId = customerId,
            TicketTypeId = ticketTypeId,
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
