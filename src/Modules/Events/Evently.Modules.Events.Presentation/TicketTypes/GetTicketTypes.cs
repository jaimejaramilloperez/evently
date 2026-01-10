using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.Application.TicketTypes.GetTicketTypes;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal static class GetTicketTypes
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (Guid eventId, ISender sender, CancellationToken cancellationToken) =>
        {
            GetTicketTypesQuery query = new(eventId);

            Result<IReadOnlyCollection<TicketTypeResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}
