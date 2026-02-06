using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Events.Application.Categories.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class UpdateCategory : IEndpoint
{
    internal sealed class Request
    {
        public required string Name { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/categories/{id:guid}", async (Guid id, Request request, ISender sender, CancellationToken cancellationToken) =>
        {
            UpdateCategoryCommand command = new()
            {
                CategoryId = id,
                Name = request.Name,
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(() => Results.Ok(), CustomResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Categories);
    }
}
