using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;
using Evently.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Modules.Ticketing.UnitTests.Orders;

public class OrderTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenOrderIsCreated()
    {
        // Arrange
        Customer customer = Customer.Create(
            Guid.CreateVersion7(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        // Act
        Result<Order> result = Order.Create(customer);

        // Assert
        OrderCreatedDomainEvent domainEvent = AssertDomainEventWasPublished<OrderCreatedDomainEvent>(result.Value);
        Assert.Equal(result.Value.Id, domainEvent.OrderId);
    }

    [Fact]
    public void IssueTicket_ShouldReturnFailure_WhenTicketAlreadyIssued()
    {
        // Arrange
        Customer customer = Customer.Create(
            Guid.CreateVersion7(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Result<Order> result = Order.Create(customer);

        result.Value.IssueTickets();
        Order order = result.Value;

        // Act
        Result issueTicketsResult = order.IssueTickets();

        // Assert
        Assert.Equivalent(OrderErrors.TicketsAlreadyIssues, issueTicketsResult.Error);
    }

    [Fact]
    public void IssueTicket_ShouldRaiseDomainEvent_WhenTicketIsIssued()
    {
        // Arrange
        Customer customer = Customer.Create(
            Guid.CreateVersion7(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Result<Order> result = Order.Create(customer);

        // Act
        result.Value.IssueTickets();

        // Assert
        OrderTicketsIssuedDomainEvent domainEvent = AssertDomainEventWasPublished<OrderTicketsIssuedDomainEvent>(result.Value);
        Assert.Equal(result.Value.Id, domainEvent.OrderId);
    }
}
