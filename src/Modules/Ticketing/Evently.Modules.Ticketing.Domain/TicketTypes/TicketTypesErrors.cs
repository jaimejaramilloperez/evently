using Evently.Common.Domain.Results;

namespace Evently.Modules.Ticketing.Domain.TicketTypes;

public static class TicketTypesErrors
{
    public static Error NotFound(Guid ticketTypeId) => Error.NotFound(
        "TicketTypes.NotFound",
        $"The ticket type with the identifier {ticketTypeId} was not found");

    public static Error NotEnoughQuantity(decimal availableQuantity) => Error.Problem(
        "TicketTypes.NotEnoughQuantity",
        $"The ticket type has {availableQuantity} quantity available");
}
