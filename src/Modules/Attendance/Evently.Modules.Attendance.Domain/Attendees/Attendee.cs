using Evently.Common.Domain;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Domain.Attendees.DomainEvents;
using Evently.Modules.Attendance.Domain.Tickets;

namespace Evently.Modules.Attendance.Domain.Attendees;

public sealed class Attendee : Entity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;

    public static Attendee Create(
        Guid id,
        string email,
        string firstName,
        string lastName)
    {
        return new()
        {
            Id = id,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };
    }

    private Attendee()
    {
    }

    public Result Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;

        return Result.Success();
    }

    public Result CheckIn(Ticket ticket)
    {
        if (Id != ticket.AttendeeId)
        {
            RaiseEvent(new InvalidCheckInAttemptedDomainEvent(Id, ticket.EventId, ticket.Id, ticket.Code));

            return Result.Failure(TicketErrors.InvalidCheckIn);
        }

        if (ticket.UsedAtUtc.HasValue)
        {
            RaiseEvent(new DuplicateCheckInAttemptedDomainEvent(Id, ticket.EventId, ticket.Id, ticket.Code));

            return Result.Failure(TicketErrors.DuplicateCheckIn);
        }

        ticket.MarkAsUsed();

        RaiseEvent(new AttendeeCheckedInDomainEvent(Id, ticket.EventId));

        return Result.Success();
    }
}
