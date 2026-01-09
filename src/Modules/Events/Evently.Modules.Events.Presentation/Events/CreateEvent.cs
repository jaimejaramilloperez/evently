using Evently.Modules.Events.Application.Events.CreateEvent;
using Evently.Modules.Events.Application.Events.GetEvents;
using Evently.Modules.Events.Domain.Abstractions.Results;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class CreateEvent
{
    internal sealed record Request
    {
        public required Guid CategoryId { get; init; }
        public required string Title { get; init; }
        public required string Description { get; init; }
        public required string Location { get; init; }
        public required DateTime StartsAtUtc { get; init; }
        public DateTime? EndsAtUtc { get; init; }
    }

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async ([FromBody] Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            CreateEventCommand command = new()
            {
                CategoryId = request.CategoryId,
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                StartsAtUtc = request.StartsAtUtc,
                EndsAtUtc = request.EndsAtUtc,
            };

            Result<EventResponse> result = await sender.Send(command, cancellationToken);

            return result.Match(() =>
                Results.Created(new Uri($"/api/events/{result.Value.Id}", UriKind.Relative), result.Value),
                CustomResults.Problem);
        });
    }
}
