using Evently.Common.Domain;
using Evently.Modules.Attendance.Domain.Events.DomainEvents;

namespace Evently.Modules.Attendance.Domain.Events;

public sealed class Event : Entity
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public DateTime StartsAtUtc { get; private set; }
    public DateTime? EndsAtUtc { get; private set; }

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

        @event.RaiseEvent(new EventCreatedDomainEvent(
            @event.Id,
            @event.Title,
            @event.Description,
            @event.Location,
            @event.StartsAtUtc,
            @event.EndsAtUtc));

        return @event;
    }

    private Event()
    {
    }
}
