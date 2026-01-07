using Evently.Modules.Events.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Events.UseCases;

public static class GetEvent
{
    public sealed record EventResponse
    {
        public required Guid Id { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required string Location { get; init; }
        public required DateTime StartsAtUtc { get; init; }
        public DateTime? EndsAtUtc { get; init; }
    }

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", async (Guid id, EventsDbContext dbContext, CancellationToken ct) =>
        {
            EventResponse? @event = await dbContext.Events.Where(x => x.Id == id)
                .Select(x => new EventResponse()
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Location = x.Location,
                    StartsAtUtc = x.StartsAtUtc,
                    EndsAtUtc = x.EndsAtUtc,
                })
                .FirstOrDefaultAsync(ct);

            return @event is null ? Results.NotFound() : Results.Ok(@event);
        });
    }
}
