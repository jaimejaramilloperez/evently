using Evently.Modules.Events.Application.Categories.CreateCategory;
using Evently.Modules.Events.Application.Categories.GetCategory;
using Evently.Modules.Events.Domain.Abstractions.Results;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal static class CreateCategory
{
    internal sealed class Request
    {
        public required string Name { get; init; }
    }

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            CreateCategoryCommand command = new(request.Name);

            Result<CategoryResponse> result = await sender.Send(command, cancellationToken);

            return result.Match(() =>
                Results.Created(new Uri($"/api/categories/{result.Value.Id}", UriKind.Relative), result.Value),
                CustomResults.Problem);
        });
    }
}
