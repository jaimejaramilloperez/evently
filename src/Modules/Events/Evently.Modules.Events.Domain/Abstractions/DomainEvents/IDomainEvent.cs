namespace Evently.Modules.Events.Domain.Abstractions.DomainEvents;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredAtUtc { get; }
}
