using Evently.Modules.Events.Application.Categories.GetCategory;
using Evently.Modules.Events.Domain.Abstractions.Results;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal static class GetCategory
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            GetCategoryQuery query = new(id);

            Result<CategoryResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}
