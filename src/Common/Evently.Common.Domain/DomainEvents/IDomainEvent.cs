using MediatR;

namespace Evently.Common.Domain.DomainEvents;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime OccurredAtUtc { get; }
}
