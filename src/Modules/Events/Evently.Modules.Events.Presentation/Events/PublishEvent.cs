using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Events.PublishEvent;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class PublishEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}/publish", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            PublishEventCommand command = new(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        });
    }
}
