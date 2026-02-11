using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Modules.Attendance.Domain.Events.DomainEvents;

namespace Evently.Modules.Attendance.Application.EventStatistics.Projections;

internal sealed class EventCreatedDomainEventHandler(IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<EventCreatedDomainEvent>
{
    public override async Task Handle(
        EventCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            """
            INSERT INTO attendance.event_statistics (
                event_id,
                title,
                description,
                location,
                starts_at_utc,
                ends_at_utc,
                tickets_sold,
                attendees_checked_in,
                duplicate_check_in_tickets,
                invalid_check_in_tickets
            )
            VALUES (
                @EventId,
                @Title,
                @Description,
                @Location,
                @StartsAtUtc,
                @EndsAtUtc,
                @TicketsSold,
                @AttendeesCheckedIn,
                @DuplicateCheckInTickets,
                @InvalidCheckInTickets
            )
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new
            {
                EventId = domainEvent.EventId,
                Title = domainEvent.Title,
                Description = domainEvent.Description,
                Location = domainEvent.Location,
                StartsAtUtc = domainEvent.StartsAtUtc,
                EndsAtUtc = domainEvent.EndsAtUtc,
                TicketsSold = 0,
                AttendeesCheckedIn = 0,
                DuplicateCheckInTickets = Array.Empty<string>(),
                InvalidCheckInTickets = Array.Empty<string>(),
            },
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }
}
