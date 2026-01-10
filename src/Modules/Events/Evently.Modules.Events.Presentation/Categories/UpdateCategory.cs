using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Categories.UpdateCategory;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal static class UpdateCategory
{
    internal sealed class Request
    {
        public required string Name { get; init; }
    }

    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}", async (Guid id, Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            UpdateCategoryCommand command = new()
            {
                CategoryId = id,
                Name = request.Name,
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(() => Results.Ok(), CustomResults.Problem);
        });
    }
}
