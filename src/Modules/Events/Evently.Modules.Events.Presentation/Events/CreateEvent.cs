using Evently.Modules.Events.Application.Events.CreateEvent;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class CreateEvent
{
    public sealed record EventRequest
    {
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required string Location { get; init; }
        public required DateTime StartsAtUtc { get; init; }
        public DateTime? EndsAtUtc { get; init; }
    }

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (EventRequest request, ISender sender, CancellationToken ct) =>
        {
            CreateEventCommand command = new()
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                StartsAtUtc = request.StartsAtUtc,
                EndsAtUtc = request.EndsAtUtc,
            };

            Guid eventId = await sender.Send(command, ct);

            return Results.Created("/events/{id}", eventId);
        });
    }
}
