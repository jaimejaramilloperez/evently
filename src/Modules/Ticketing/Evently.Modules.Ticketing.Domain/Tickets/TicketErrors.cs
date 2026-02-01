using Evently.Common.Domain.Results;

namespace Evently.Modules.Ticketing.Domain.Tickets;

public static class TicketErrors
{
    public static Error NotFound(Guid ticketTypeId) => Error.NotFound(
        "TicketTypes.NotFound",
        $"The ticket type with the identifier {ticketTypeId} was not found");

    public static Error NotFound(string code) => Error.NotFound(
        "TicketTypes.NotFound",
        $"The ticket type with the identifier {code} was not found");

    public static Error NotEnoughQuantity(decimal availableQuantity) => Error.Problem(
        "TicketTypes.NotEnoughQuantity",
        $"The ticket type has {availableQuantity} quantity available");
}
