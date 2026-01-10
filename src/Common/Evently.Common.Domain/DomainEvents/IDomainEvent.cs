namespace Evently.Common.Domain.DomainEvents;

public interface IDomainEvent
{
    Guid Id { get; }
    DateTime OccurredAtUtc { get; }
}
