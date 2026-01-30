using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.PublicApi;
using MediatR;
using TicketTypeResponse = Evently.Modules.Events.PublicApi.TicketTypeResponse;

namespace Evently.Modules.Events.Infrastructure.PublicApi;

internal sealed class EventsApi(ISender sender) : IEventsApi
{
    public async Task<TicketTypeResponse?> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken = default)
    {
        GetTicketTypeQuery query = new(ticketTypeId);

        Result<Application.TicketTypes.GetTicketType.TicketTypeResponse> result = await sender.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return null;
        }

        return new()
        {
            Id = result.Value.Id,
            EventId = result.Value.EventId,
            Name = result.Value.Name,
            Price = result.Value.Price,
            Currency = result.Value.Currency,
            Quantity = result.Value.Quantity,
        };
    }
}
