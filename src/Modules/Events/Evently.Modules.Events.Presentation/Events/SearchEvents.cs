using Evently.Modules.Events.Application.Events.SearchEvents;
using Evently.Modules.Events.Domain.Abstractions.Results;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal static class SearchEvents
{
    internal sealed record Request
    {
        [FromQuery(Name = "category_id")]
        public Guid? CategoryId { get; init; }

        [FromQuery(Name = "start_date")]
        public DateTime? StartDate { get; init; }

        [FromQuery(Name = "end_date")]
        public DateTime? EndDate { get; init; }

        [FromQuery]
        public int Page { get; init; } = 0;

        [FromQuery(Name = "page_size")]
        public int PageSize { get; init; } = 15;
    }

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/search", async ([AsParameters] Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            SearchEventsQuery query = new()
            {
                CategoryId = request.CategoryId,
                StartDate = request.StartDate,
                PageSize = request.PageSize,
                Page = request.PageSize,
                EndDate = request.EndDate,
            };

            Result<SearchEventsResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}
