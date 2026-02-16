using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Domain.Attendees;

namespace Evently.Modules.Attendance.Application.Attendees.GetAttendee;

internal sealed class GetAttendeeQueryQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetAttendeeQuery, AttendeeResponse>
{
    public async Task<Result<AttendeeResponse>> Handle(GetAttendeeQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
                id AS {nameof(AttendeeResponse.Id)},
                email AS {nameof(AttendeeResponse.Email)},
                first_name AS {nameof(AttendeeResponse.FirstName)},
                last_name AS {nameof(AttendeeResponse.LastName)}
            FROM
                attendance.attendees
            WHERE
                id = @CustomerId
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { CustomerId = request.CustomerId },
            cancellationToken: cancellationToken);

        AttendeeResponse? attendee = await connection.QuerySingleOrDefaultAsync<AttendeeResponse>(command);

        if (attendee is null)
        {
            return Result.Failure<AttendeeResponse>(AttendeeErrors.NotFound(request.CustomerId));
        }

        return attendee;
    }
}
