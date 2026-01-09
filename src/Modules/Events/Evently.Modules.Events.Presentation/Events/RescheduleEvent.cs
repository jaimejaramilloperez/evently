using Evently.Modules.Events.Application.Events.RescheduleEvent;
using Evently.Modules.Events.Domain.Abstractions.Results;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class RescheduleEvent
{
    internal sealed class Request
    {
        public required DateTime StartsAtUtc { get; init; }
        public DateTime? EndsAtUtc { get; init; }
    }

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}/reschedule", async (Guid id, Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            RescheduleEventCommand command = new()
            {
                EventId = id,
                StartsAtUtc = request.StartsAtUtc,
                EndsAtUtc = request.EndsAtUtc,
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        });
    }
}
