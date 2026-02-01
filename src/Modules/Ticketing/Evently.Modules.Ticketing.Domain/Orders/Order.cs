using Evently.Common.Domain;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Orders;

public sealed class Order : Entity
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal TotalPrice { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public bool TicketsIssued { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public ICollection<OrderItem> OrderItems { get; init; } = [];

    public static Guid CreateOrderId()
    {
        return Guid.CreateVersion7();
    }

    public static Order Create(Customer customer)
    {
        Order order = new()
        {
            Id = CreateOrderId(),
            CustomerId = customer.Id,
            Status = OrderStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow,
        };

        order.RaiseEvent(new OrderCreatedDomainEvent(order.Id));

        return order;
    }

    private Order()
    {
    }

    public Result AddItem(TicketType ticketType, decimal quantity, decimal price, string currency)
    {
        OrderItem orderItem = OrderItem.Create(Id, ticketType.Id, quantity, price, currency);

        OrderItems.Add(orderItem);

        TotalPrice = OrderItems.Sum(o => o.Price);
        Currency = currency;

        return Result.Success();
    }

    public Result IssueTickets()
    {
        if (TicketsIssued)
        {
            return Result.Failure(OrderErrors.TicketsAlreadyIssues);
        }

        TicketsIssued = true;

        RaiseEvent(new OrderTicketsIssuedDomainEvent(Id));

        return Result.Success();
    }
}
