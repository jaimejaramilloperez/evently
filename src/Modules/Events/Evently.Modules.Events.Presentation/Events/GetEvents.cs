using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.Events.GetEvents;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal sealed class GetEvents : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/events", async (ISender sender, CancellationToken cancellationToken) =>
        {
            GetEventsQuery query = new();

            Result<IReadOnlyCollection<EventResponse>> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Events);
    }
}
