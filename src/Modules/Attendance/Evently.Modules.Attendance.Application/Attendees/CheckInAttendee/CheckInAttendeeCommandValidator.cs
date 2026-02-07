using FluentValidation;

namespace Evently.Modules.Attendance.Application.Attendees.CheckInAttendee;

internal sealed class CheckInAttendeeCommandValidator : AbstractValidator<CheckInAttendeeCommand>
{
    public CheckInAttendeeCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}
