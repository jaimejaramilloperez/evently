using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Attendance.Application.Abstractions.Authentication;
using Evently.Modules.Attendance.Application.Attendees.CheckInAttendee;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Attendance.Presentation.Attendees;

internal sealed class CheckInAttendee : IEndpoint
{
    internal sealed class Request
    {
        public Guid TicketId { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/attendees/check-in", async (Request request, IAttendanceContext attendanceContext, ISender sender, CancellationToken cancellationToken = default) =>
        {
            CheckInAttendeeCommand command = new()
            {
                AttendeeId = attendanceContext.AttendeeId,
                TicketId = request.TicketId,
            };

            Result result = await sender.Send(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .RequireAuthorization(Permissions.CheckInTicket)
        .WithTags(Tags.Attendees);
    }
}
