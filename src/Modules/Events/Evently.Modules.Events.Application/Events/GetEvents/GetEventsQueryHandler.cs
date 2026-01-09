using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Domain.Abstractions.Results;

namespace Evently.Modules.Events.Application.Events.GetEvents;

internal sealed class GetEventsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetEventsQuery, IReadOnlyCollection<EventResponse>>
{
    public async Task<Result<IReadOnlyCollection<EventResponse>>> Handle(
        GetEventsQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

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
             """;

        CommandDefinition command = new(
            commandText: sql,
            cancellationToken: cancellationToken);

        IEnumerable<EventResponse> events = await connection.QueryAsync<EventResponse>(command);

        return events.AsList();
    }
}
