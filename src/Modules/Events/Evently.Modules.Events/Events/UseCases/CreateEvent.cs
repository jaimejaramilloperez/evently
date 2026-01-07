using Evently.Modules.Events.Database;
using Evently.Modules.Events.Events.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Events.UseCases;

public static class CreateEvent
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
        app.MapPost("/", async (EventRequest request, EventsDbContext dbContext, CancellationToken ct) =>
        {
            Event @event = new()
            {
                Id = Guid.CreateVersion7(),
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                StartsAtUtc = request.StartsAtUtc,
                EndsAtUtc = request.EndsAtUtc,
                Status = EventStatus.Draft,
            };

            dbContext.Add(@event);

            await dbContext.SaveChangesAsync(ct);

            return Results.Created("/events/{id}", @event.Id);
        });
    }
}
