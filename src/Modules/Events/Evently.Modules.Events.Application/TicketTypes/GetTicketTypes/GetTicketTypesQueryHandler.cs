using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;

namespace Evently.Modules.Events.Application.TicketTypes.GetTicketTypes;

internal sealed class GetTicketTypesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketTypesQuery, IReadOnlyCollection<TicketTypeResponse>>
{
    public async Task<Result<IReadOnlyCollection<TicketTypeResponse>>> Handle(
        GetTicketTypesQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
                id AS {nameof(TicketTypeResponse.Id)},
                event_id AS {nameof(TicketTypeResponse.EventId)},
                name AS {nameof(TicketTypeResponse.Name)},
                price AS {nameof(TicketTypeResponse.Price)},
                currency AS {nameof(TicketTypeResponse.Currency)},
                quantity AS {nameof(TicketTypeResponse.Quantity)}
            FROM
                events.ticket_types
            WHERE
                event_id = @EventId
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { request.EventId },
            cancellationToken: cancellationToken);

        IEnumerable<TicketTypeResponse> ticketTypes = await dbConnection.QueryAsync<TicketTypeResponse>(command);

        return ticketTypes.AsList();
    }
}
