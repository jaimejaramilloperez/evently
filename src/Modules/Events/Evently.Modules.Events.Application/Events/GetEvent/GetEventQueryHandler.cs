using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.GetEvent;

internal sealed class GetEventQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetEventQuery, EventResponse?>
{
    public async Task<Result<EventResponse?>> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection dbConnection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
                e.id AS {nameof(EventResponse.Id)},
                e.category_id AS {nameof(EventResponse.CategoryId)},
                e.title AS {nameof(EventResponse.Title)},
                e.description AS {nameof(EventResponse.Description)},
                e.location AS {nameof(EventResponse.Location)},
                e.starts_at_utc AS {nameof(EventResponse.StartsAtUtc)},
                e.ends_at_utc AS {nameof(EventResponse.EndsAtUtc)},
                tt.id AS {nameof(TicketTypeResponse.TicketTypeId)},
                tt.name AS {nameof(TicketTypeResponse.Name)},
                tt.price AS {nameof(TicketTypeResponse.Price)},
                tt.currency AS {nameof(TicketTypeResponse.Currency)},
                tt.quantity AS {nameof(TicketTypeResponse.Quantity)}
             FROM
                events.events AS e
             LEFT JOIN
                events.ticket_types AS tt ON tt.event_id = e.id
             WHERE
                e.id = @EventId
            """;

        Dictionary<Guid, EventResponse> eventsDictionary = [];

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { request.EventId },
            cancellationToken: cancellationToken);

        await dbConnection.QueryAsync<EventResponse, TicketTypeResponse?, EventResponse>(
            command: command,
            map: (@event, ticketType) =>
            {
                if (eventsDictionary.TryGetValue(@event.Id, out EventResponse? existingEvent))
                {
                    @event = existingEvent;
                }
                else
                {
                    eventsDictionary.Add(@event.Id, @event);
                }

                if (ticketType is not null)
                {
                    @event.TicketTypes.Add(ticketType);
                }

                return @event;
            },
            splitOn: nameof(TicketTypeResponse.TicketTypeId));

        if (!eventsDictionary.TryGetValue(request.EventId, out EventResponse? eventResponse))
        {
            return Result.Failure<EventResponse?>(EventErrors.NotFound(request.EventId));
        }

        return eventResponse;
    }
}
