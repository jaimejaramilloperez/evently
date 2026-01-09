using Evently.Modules.Events.Domain.Abstractions.Results;

namespace Evently.Modules.Events.Domain.TicketTypes;

public static class TicketTypeErrors
{
    public static Error NotFound(Guid ticketTypeId) => Error.NotFound(
        "TicketTypes.NotFound",
        $"The ticket type with the identifier {ticketTypeId} was not found");

    public static readonly Error PriceLowerThanZero = Error.Problem(
        "TicketTypes.PriceLowerThanZero",
        $"The ticket price can not be lower than zero");
}
