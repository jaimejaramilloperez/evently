using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Events.DomainEvents;

namespace Evently.Modules.Events.Domain.Events;

public sealed class Event : Entity
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public DateTime StartsAtUtc { get; private set; }
    public DateTime? EndsAtUtc { get; private set; }
    public EventStatus Status { get; private set; }

    public Event()
    {
    }

    public static Guid CreateEventId()
    {
        return Guid.CreateVersion7();
    }

    public static Event Create(
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        Event @event = new()
        {
            Id = CreateEventId(),
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
            Status = EventStatus.Draft,
        };

        @event.RaiseEvent(new EventCanceledDomainEvent(@event.Id));

        return @event;
    }
}
