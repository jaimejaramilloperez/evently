namespace Evently.Common.Application.EventBus;

public abstract class IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTime OccurredAtUtc { get; init; }

    protected IntegrationEvent()
    {
        Id = Guid.CreateVersion7();
        OccurredAtUtc = DateTime.UtcNow;
    }

    protected IntegrationEvent(Guid id, DateTime occurredAtUtc)
    {
        Id = id;
        OccurredAtUtc = occurredAtUtc;
    }
}
