using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Events.GetEvents;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.SearchEvents;

internal sealed class SearchEventsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<SearchEventsQuery, SearchEventsResponse>
{
    public async Task<Result<SearchEventsResponse>> Handle(
        SearchEventsQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        SearchEventsParameters parameters = new()
        {
            CategoryId = request.CategoryId,
            Status = (int)EventStatus.Published,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Take = request.PageSize,
            Skip = (request.Page - 1) * request.PageSize,
        };

        IReadOnlyCollection<EventResponse> events = await GetEventsAsync(dbConnection, parameters, cancellationToken);

        int totalCount = await CountEventsAsync(dbConnection, parameters, cancellationToken);

        return new SearchEventsResponse()
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            Events = events,
        };
    }

    private static async Task<IReadOnlyCollection<EventResponse>> GetEventsAsync(
        DbConnection connection,
        SearchEventsParameters parameters,
        CancellationToken cancellationToken = default)
    {
        const string sql =
            $"""
             SELECT
                id AS {nameof(EventResponse.Id)},
                category_id AS {nameof(EventResponse.CategoryId)},
                title AS {nameof(EventResponse.Title)},
                description AS {nameof(EventResponse.Description)},
                location AS {nameof(EventResponse.Location)},
                starts_at_utc AS {nameof(EventResponse.StartsAtUtc)},
                ends_at_utc AS {nameof(EventResponse.EndsAtUtc)}
             FROM
                events.events
             WHERE
                status = @Status AND
                (@CategoryId IS NULL OR category_id = @CategoryId) AND
                (@StartDate::timestamp IS NULL OR starts_at_utc >= @StartDate::timestamp) AND
                (@EndDate::timestamp IS NULL OR ends_at_utc >= @EndDate::timestamp)
             ORDER BY
                starts_at_utc, id
             OFFSET @Skip
             LIMIT @Take
             """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: parameters,
            cancellationToken: cancellationToken);

        IEnumerable<EventResponse> events = await connection.QueryAsync<EventResponse>(command);

        return events.AsList();
    }

    private static async Task<int> CountEventsAsync(
        DbConnection connection,
        SearchEventsParameters parameters,
        CancellationToken cancellationToken = default)
    {
        const string sql =
            """
            SELECT
                COUNT(*)
            FROM
                events.events
            WHERE
               status = @Status AND
               (@CategoryId IS NULL OR category_id = @CategoryId) AND
               (@StartDate::timestamp IS NULL OR starts_at_utc >= @StartDate::timestamp) AND
               (@EndDate::timestamp IS NULL OR ends_at_utc >= @EndDate::timestamp)
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: parameters,
            cancellationToken: cancellationToken);

        int totalCount = await connection.ExecuteScalarAsync<int>(command);

        return totalCount;
    }

    private sealed record SearchEventsParameters
    {
        public Guid? CategoryId { get; init; }
        public int Status { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public int Take { get; init; }
        public int Skip { get; init; }
    }
}
