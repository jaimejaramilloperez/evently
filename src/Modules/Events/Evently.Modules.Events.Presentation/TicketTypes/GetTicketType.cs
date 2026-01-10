using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal static class GetTicketType
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            GetTicketTypeQuery query = new(id);

            Result<TicketTypeResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}
