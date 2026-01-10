using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Events.CancelEvent;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class CancelEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id:guid}/cancel", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            CancelEventCommand command = new(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        });
    }
}
