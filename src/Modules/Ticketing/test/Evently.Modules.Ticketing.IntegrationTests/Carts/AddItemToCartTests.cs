using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Carts.AddItemToCart;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.IntegrationTests.Abstractions;

namespace Evently.Modules.Ticketing.IntegrationTests.Carts;

public class AddItemToCartTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private const decimal Quantity = 10;

    [Fact]
    public async Task Should_ReturnFailure_WhenCustomerDoesNotExist()
    {
        // Arrange
        AddItemToCartCommand command = new()
        {
            CustomerId = Guid.CreateVersion7(),
            TicketTypeId = Guid.CreateVersion7(),
            Quantity = Faker.Random.Decimal(),
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

        AddItemToCartCommand command = new()
        {
            CustomerId = customerId,
            TicketTypeId = Guid.CreateVersion7(),
            Quantity = Faker.Random.Decimal(),
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(TicketTypeErrors.NotFound(command.TicketTypeId), result.Error);
    }

    [Fact]
    public async Task Should_ReturnFailure_WhenNotEnoughQuantity()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid customerId = await CreateCustomerAsync(Guid.CreateVersion7(), cancellationToken);
        var eventId = Guid.CreateVersion7();
        var ticketTypeId = Guid.CreateVersion7();

        await CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity, cancellationToken);

        AddItemToCartCommand command = new()
        {
            CustomerId = customerId,
            TicketTypeId = ticketTypeId,
            Quantity = Quantity + 1,
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.Equivalent(TicketTypeErrors.NotEnoughQuantity(Quantity), result.Error);
    }

    [Fact]
    public async Task Should_ReturnSuccess_WhenItemAddedToCart()
    {
        // Arrange
        CancellationToken cancellationToken = TestContext.Current.CancellationToken;

        Guid customerId = await CreateCustomerAsync(Guid.CreateVersion7(), cancellationToken);
        var eventId = Guid.CreateVersion7();
        var ticketTypeId = Guid.CreateVersion7();

        await CreateEventWithTicketTypeAsync(eventId, ticketTypeId, Quantity, cancellationToken);

        AddItemToCartCommand command = new()
        {
            CustomerId = customerId,
            TicketTypeId = ticketTypeId,
            Quantity = Quantity,
        };

        // Act
        Result result = await SendAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
