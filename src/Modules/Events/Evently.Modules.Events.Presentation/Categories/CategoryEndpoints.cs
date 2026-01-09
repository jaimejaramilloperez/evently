using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

public static class CategoryEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder categoriesGroup = app.MapGroup("/categories")
            .WithTags(Tags.Categories);

        ArchiveCategory.MapEndpoint(categoriesGroup);
        CreateCategory.MapEndpoint(categoriesGroup);
        GetCategory.MapEndpoint(categoriesGroup);
        GetCategories.MapEndpoint(categoriesGroup);
        UpdateCategory.MapEndpoint(categoriesGroup);
    }
}
