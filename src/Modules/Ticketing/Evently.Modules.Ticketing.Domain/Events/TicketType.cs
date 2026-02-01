using Evently.Common.Domain;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Events;

public sealed class TicketType : Entity
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; }
    public decimal AvailableQuantity { get; private set; }

    public static TicketType Create(
        Guid id,
        Guid eventId,
        string name,
        decimal price,
        string currency,
        decimal quantity)
    {
        TicketType ticketType = new()
        {
            Id = id,
            EventId = eventId,
            Name = name,
            Price = price,
            Currency = currency,
            Quantity = quantity,
            AvailableQuantity = quantity,
        };

        return ticketType;
    }

    private TicketType()
    {
    }

    public Result UpdatePrice(decimal price)
    {
        Price = price;
        return Result.Success();
    }

    public Result UpdateQuantity(decimal quantity)
    {
        if (quantity > AvailableQuantity)
        {
            return Result.Failure(TicketTypeErrors.NotEnoughQuantity(AvailableQuantity));
        }

        AvailableQuantity -= quantity;

        if (AvailableQuantity == 0)
        {
            RaiseEvent(new TicketTypeSoldOutDomainEvent(Id));
        }

        return Result.Success();
    }
}
