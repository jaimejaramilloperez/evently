using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Application.Categories.GetCategory;

internal sealed class GetCategoryQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetCategoryQuery, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                id AS {nameof(CategoryResponse.Id)},
                name AS {nameof(CategoryResponse.Name)},
                is_archived AS {nameof(CategoryResponse.IsArchived)}
             FROM
                events.categories
             WHERE
                id = @CategoryId
             """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { request.CategoryId },
            cancellationToken: cancellationToken);

        CategoryResponse? category = await dbConnection.QuerySingleOrDefaultAsync<CategoryResponse>(command);

        if (category is null)
        {
            return Result.Failure<CategoryResponse>(CategoryErrors.NotFound(request.CategoryId));
        }

        return category;
    }
}
