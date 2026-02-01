using Evently.Common.Domain;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;

namespace Evently.Modules.Ticketing.Domain.Events;

public sealed class Event : Entity
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public DateTime StartsAtUtc { get; private set; }
    public DateTime? EndsAtUtc { get; private set; }
    public bool Canceled { get; private set; }

    public static Event Create(
        Guid id,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        Event @event = new()
        {
            Id = id,
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
        };

        return @event;
    }

    private Event()
    {
    }

    public Result Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
    {
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;

        RaiseEvent(new EventRescheduledDomainEvent(Id, StartsAtUtc, EndsAtUtc));

        return Result.Success();
    }

    public Result Cancel()
    {
        if (Canceled)
        {
            return Result.Success();
        }

        Canceled = true;

        RaiseEvent(new EventCanceledDomainEvent(Id));

        return Result.Success();
    }

    public Result PaymentsRefunded()
    {
        RaiseEvent(new EventPaymentsRefundedDomainEvent(Id));
        return Result.Success();
    }

    public Result TicketsArchived()
    {
        RaiseEvent(new EventTicketsArchivedDomainEvent(Id));
        return Result.Success();
    }
}
