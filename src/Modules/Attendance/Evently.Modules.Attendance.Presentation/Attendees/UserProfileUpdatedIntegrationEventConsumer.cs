using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Attendees.UpdateAttendee;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Attendees;

public sealed class UserProfileUpdatedIntegrationEventConsumer(ISender sender)
    : IConsumer<UserProfileUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserProfileUpdatedIntegrationEvent> context)
    {
        UpdateAttendeeCommand command = new()
        {
            AttendeeId = context.Message.UserId,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName,
        };

        Result result = await sender.Send(command, context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(UpdateAttendeeCommand), result.Error);
        }
    }
}
