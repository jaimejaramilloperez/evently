using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.Categories.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class GetCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/categories/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            GetCategoryQuery query = new(id);

            Result<CategoryResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Categories);
    }
}
