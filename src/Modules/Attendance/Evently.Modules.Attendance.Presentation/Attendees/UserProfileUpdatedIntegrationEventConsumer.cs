using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Attendees.UpdateAttendee;
using Evently.Modules.Users.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Attendees;

public sealed class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IntegrationEventHandler<UserProfileUpdatedIntegrationEvent>
{
    public override async Task Handle(
        UserProfileUpdatedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        UpdateAttendeeCommand command = new()
        {
            AttendeeId = integrationEvent.UserId,
            FirstName = integrationEvent.FirstName,
            LastName = integrationEvent.LastName,
        };

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(UpdateAttendeeCommand), result.Error);
        }
    }
}
