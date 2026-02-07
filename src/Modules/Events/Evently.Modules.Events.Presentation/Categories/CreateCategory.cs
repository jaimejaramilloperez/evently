using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.Categories.CreateCategory;
using Evently.Modules.Events.Application.Categories.GetCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class CreateCategory : IEndpoint
{
    internal sealed class Request
    {
        public required string Name { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/categories", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            CreateCategoryCommand command = new(request.Name);

            Result<CategoryResponse> result = await sender.Send(command, cancellationToken);

            return result.Match(() =>
                Results.Created(new Uri($"/api/categories/{result.Value.Id}", UriKind.Relative), result.Value),
                CustomResults.Problem);
        })
        .RequireAuthorization(Permissions.ModifyCategories)
        .WithTags(Tags.Categories);
    }
}
