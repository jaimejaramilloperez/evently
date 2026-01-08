using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Application.Abstractions.Data;
using MediatR;

namespace Evently.Modules.Events.Application.Events.GetEvent;

internal sealed class GetEventQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IRequestHandler<GetEventQuery, EventResponse?>
{
    public async Task<EventResponse?> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
                id AS {nameof(EventResponse.Id)}
                title AS {nameof(EventResponse.Title)}
                description AS {nameof(EventResponse.Description)}
                location AS {nameof(EventResponse.Location)}
                starts_at_utc AS {nameof(EventResponse.StartsAtUtc)}
                ends_at_utc AS {nameof(EventResponse.EndsAtUtc)}
            FROM events.events
            WHERE id = @EventId
            """;

        CommandDefinition command = new(sql, request, cancellationToken: cancellationToken);

        EventResponse? response = await dbConnection.QueryFirstOrDefaultAsync<EventResponse>(command);

        return response;
    }
}
