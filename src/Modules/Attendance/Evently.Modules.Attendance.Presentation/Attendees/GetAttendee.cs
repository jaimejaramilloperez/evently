using Evently.Common.Domain.Results;
using Evently.Common.Presentation.ApiResults;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Attendance.Application.Abstractions.Authentication;
using Evently.Modules.Attendance.Application.Attendees.GetAttendee;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Attendance.Presentation.Attendees;

internal sealed class GetAttendee : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/attendees/profile", async (IAttendanceContext attendanceContext, ISender sender, CancellationToken cancellationToken) =>
        {
            GetAttendeeQuery query = new(attendanceContext.AttendeeId);

            Result<AttendeeResponse> result = await sender.Send(query, cancellationToken);

            return result.Match(() => Results.Ok(result.Value), CustomResults.Problem);
        })
        .RequireAuthorization(Permissions.GetAttendeeProfile)
        .WithTags(Tags.Attendees);
    }
}
