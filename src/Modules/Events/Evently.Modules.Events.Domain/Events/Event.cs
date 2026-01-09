using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Abstractions.Results;
using Evently.Modules.Events.Domain.Events.DomainEvents;

namespace Evently.Modules.Events.Domain.Events;

public sealed class Event : Entity
{
    public Guid Id { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public DateTime StartsAtUtc { get; private set; }
    public DateTime? EndsAtUtc { get; private set; }
    public EventStatus Status { get; private set; }

    public static Guid CreateEventId()
    {
        return Guid.CreateVersion7();
    }

    public static Result<Event> Create(
        TimeProvider timeProvider,
        Guid categoryId,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        if (startsAtUtc < timeProvider.GetUtcNow().UtcDateTime.Subtract(EventConstants.AllowedClockSkew))
        {
            return Result.Failure<Event>(EventErrors.StartDateInPast);
        }

        if (endsAtUtc.HasValue && endsAtUtc < startsAtUtc)
        {
            return Result.Failure<Event>(EventErrors.EndDatePrecedesStartDate);
        }

        Event @event = new()
        {
            Id = CreateEventId(),
            CategoryId = categoryId,
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
            Status = EventStatus.Draft,
        };

        @event.RaiseEvent(new EventCreatedDomainEvent(@event.Id));

        return @event;
    }

    private Event()
    {
    }

    public Result Publish()
    {
        if (Status != EventStatus.Draft)
        {
            return Result.Failure(EventErrors.NotDraft);
        }

        Status = EventStatus.Published;

        RaiseEvent(new EventPublishedDomainEvent(Id));

        return Result.Success();
    }

    public Result Reschedule(TimeProvider timeProvider, DateTime startsAtUtc, DateTime? endsAtUtc)
    {
        if (startsAtUtc < timeProvider.GetUtcNow().UtcDateTime.Subtract(EventConstants.AllowedClockSkew))
        {
            return Result.Failure<Event>(EventErrors.StartDateInPast);
        }

        if (StartsAtUtc == startsAtUtc && EndsAtUtc == endsAtUtc)
        {
            return Result.Success();
        }

        if (endsAtUtc.HasValue && endsAtUtc < startsAtUtc)
        {
            return Result.Failure<Event>(EventErrors.EndDatePrecedesStartDate);
        }

        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;

        RaiseEvent(new EventRescheduledDomainEvent(Id, StartsAtUtc, EndsAtUtc));

        return Result.Success();
    }

    public Result Cancel(TimeProvider timeProvider)
    {
        if (StartsAtUtc < timeProvider.GetUtcNow().UtcDateTime)
        {
            return Result.Failure(EventErrors.AlreadyStarted);
        }

        if (Status == EventStatus.Canceled)
        {
            return Result.Failure(EventErrors.AlreadyCanceled);
        }

        Status = EventStatus.Canceled;

        RaiseEvent(new EventCanceledDomainEvent(Id));

        return Result.Success();
    }
}
