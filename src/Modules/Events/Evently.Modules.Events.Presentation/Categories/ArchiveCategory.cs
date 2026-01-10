using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Categories.ArchiveCategory;
using Evently.Modules.Events.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

internal static class ArchiveCategory
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}/archive", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            ArchiveCategoryCommand command = new(id);

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(() => Results.Ok(), CustomResults.Problem);
        });
    }
}
