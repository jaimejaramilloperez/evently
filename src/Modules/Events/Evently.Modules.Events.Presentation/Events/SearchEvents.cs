using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.Events.SearchEvents;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

internal sealed class SearchEvents : IEndpoint
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
        public int Page { get; init; } = 1;

        [FromQuery(Name = "page_size")]
        public int PageSize { get; init; } = 15;
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/events/search", async ([AsParameters] Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            SearchEventsQuery query = new()
            {
                CategoryId = request.CategoryId,
                StartDate = request.StartDate,
                Page = request.Page,
                PageSize = request.PageSize,
                EndDate = request.EndDate,
            };

            Result<SearchEventsResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Events);
    }
}
