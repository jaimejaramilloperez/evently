namespace Evently.Common.Domain.DomainEvents;

public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; init; }

    public DateTime OccurredAtUtc { get; init; }

    protected DomainEvent()
    {
        Id = Guid.CreateVersion7();
        OccurredAtUtc = DateTime.UtcNow;
    }

    protected DomainEvent(Guid id, DateTime occurredAtUtc)
    {
        Id = id;
        OccurredAtUtc = occurredAtUtc;
    }
}
