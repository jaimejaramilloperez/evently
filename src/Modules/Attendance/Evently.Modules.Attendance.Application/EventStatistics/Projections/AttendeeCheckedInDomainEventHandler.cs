using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class AttendeeCheckedInDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<AttendeeCheckedInDomainEvent>
{
    public override async Task Handle(
        AttendeeCheckedInDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            UPDATE
                attendance.event_statistics AS es
            SET attendees_checked_in = (
                SELECT
                    COUNT(*)
                FROM
                    attendance.tickets AS t
                WHERE
                    t.event_id = es.event_id AND
                    t.used_at_utc IS NOT NULL
            )
            WHERE
                es.event_id = @EventId
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { EventId = domainEvent.EventId },
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }
}
