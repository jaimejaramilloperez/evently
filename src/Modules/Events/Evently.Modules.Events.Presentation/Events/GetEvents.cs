using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Events.GetEvents;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class GetEvents
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (ISender sender, CancellationToken cancellationToken) =>
        {
            GetEventsQuery query = new();

            Result<IReadOnlyCollection<EventResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}
