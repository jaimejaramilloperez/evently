using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Attendees.CreateAttendee;
using Evently.Modules.Users.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Attendees;

public sealed class UserRegisteredIntegrationEventConsumer(ISender sender)
    : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(
        UserRegisteredIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        CreateAttendeeCommand command = new()
        {
            AttendeeId = integrationEvent.UserId,
            Email = integrationEvent.Email,
            FirstName = integrationEvent.FirstName,
            LastName = integrationEvent.LastName,
        };

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CreateAttendeeCommand), result.Error);
        }
    }
}
