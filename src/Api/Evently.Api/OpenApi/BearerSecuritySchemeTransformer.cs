using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Evently.Api.OpenApi;

public sealed class BearerSecuritySchemeTransformer() : IOpenApiDocumentTransformer
{
    public Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        Dictionary<string, IOpenApiSecurityScheme> securitySchemes = new()
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using Bearer scheme",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
            },
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = securitySchemes;

        IEnumerable<KeyValuePair<HttpMethod, OpenApiOperation>> operations = document.Paths.Values
            .SelectMany(x => x.Operations ?? []);

        foreach (KeyValuePair<HttpMethod, OpenApiOperation> operation in operations)
        {
            operation.Value.Security ??= [];

            operation.Value.Security.Add(new()
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
            });
        }

        return Task.CompletedTask;
    }
}
