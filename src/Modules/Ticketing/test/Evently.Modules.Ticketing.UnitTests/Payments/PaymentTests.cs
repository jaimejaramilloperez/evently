using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;
using Evently.Modules.Ticketing.Domain.Payments.DomainEvents;
using Evently.Modules.Ticketing.UnitTests.Abstractions;

namespace Evently.Modules.Ticketing.UnitTests.Payments;

public class PaymentTests : BaseTest
{
    [Fact]
    public void Create_ShouldRaiseDomainEvent_WhenPaymentIsCreated()
    {
        // Arrange
        Customer customer = Customer.Create(
            Guid.CreateVersion7(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Order order = Order.Create(customer);

        // Act
        Result<Payment> result = Payment.Create(
            order,
            Guid.CreateVersion7(),
            Faker.Random.Decimal(),
            Faker.Random.AlphaNumeric(10));

        // Assert
        PaymentCreatedDomainEvent domainEvent = AssertDomainEventWasPublished<PaymentCreatedDomainEvent>(result.Value);
        Assert.Equal(result.Value.Id, domainEvent.PaymentId);
    }

    [Fact]
    public void Refund_ShouldReturnFailure_WhenAlreadyRefunded()
    {
        // Arrange
        decimal amount = Faker.Random.Decimal();

        Customer customer = Customer.Create(
            Guid.CreateVersion7(),
            Faker.Internet.Email(),
            Faker.Name.FirstName(),
            Faker.Name.LastName());

        Order order = Order.Create(customer);

        Result<Payment> paymentResult = Payment.Create(
            order,
            Guid.CreateVersion7(),
            amount,
            Faker.Random.AlphaNumeric(10));

        Payment payment = paymentResult.Value;

        payment.Refund(amount);

        // Act
        Result result = payment.Refund(amount);

        // Assert
        Assert.Equivalent(PaymentErrors.AlreadyRefunded, result.Error);
    }
}
