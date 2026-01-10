using Evently.Common.Domain;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.Domain.TicketTypes.DomainEvents;

namespace Evently.Modules.Events.Domain.TicketTypes;

public sealed class TicketType : Entity
{
    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public decimal Quantity { get; private set; }

    public static Guid CreateTicketTypeId()
    {
        return Guid.CreateVersion7();
    }

    public static TicketType Create(
        Guid eventid,
        string name,
        decimal price,
        string currency,
        decimal quantity)
    {
        TicketType ticketType = new()
        {
            Id = CreateTicketTypeId(),
            EventId = eventid,
            Name = name,
            Price = price,
            Currency = currency,
            Quantity = quantity,
        };

        ticketType.RaiseEvent(new TicketTypeCreatedDomainEvent(ticketType.Id));

        return ticketType;
    }

    private TicketType()
    {
    }

    public Result UpdatePrice(decimal price)
    {
        if (Price == price)
        {
            return Result.Success();
        }

        if (price < 0m)
        {
            return Result.Failure(TicketTypeErrors.PriceLowerThanZero);
        }

        Price = price;

        RaiseEvent(new TicketTypePriceChangedDomainEvent(Id, price));

        return Result.Success();
    }
}
