using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class DuplicateCheckInAttemptedDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<DuplicateCheckInAttemptedDomainEvent>
{
    public override async Task Handle(
        DuplicateCheckInAttemptedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            UPDATE
                attendance.event_statistics AS es
            SET
                duplicate_check_in_tickets = array_append(duplicate_check_in_tickets, @TicketCode)
            WHERE
                es.event_id = @EventId
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new
            {
                EventId = domainEvent.EventId,
                TicketCode = domainEvent.TicketCode,
            },
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }
}
