using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Attendees.CreateAttendee;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Attendees;

public sealed class UserRegisteredIntegrationEventConsumer(ISender sender)
    : IConsumer<UserRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        CreateAttendeeCommand command = new()
        {
            AttendeeId = context.Message.UserId,
            Email = context.Message.Email,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName,
        };

        Result result = await sender.Send(command, context.CancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CreateAttendeeCommand), result.Error);
        }
    }
}
