using Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;
using Evently.Modules.Events.Domain.Abstractions.Results;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal static class ChangeTicketTypePrice
{
    internal sealed class Request
    {
        public required decimal Price { get; init; }
    }

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id}/price", async (Guid id, Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            UpdateTicketTypePriceCommand command = new()
            {
                TicketTypeId = id,
                Price = request.Price,
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        });
    }
}
