using Evently.Modules.Events.Application.Abstractions.Messaging;

namespace Evently.Modules.Events.Application.Events.RescheduleEvent;

public sealed record RescheduleEventCommand : ICommand
{
    public required Guid EventId { get; init; }
    public required DateTime StartsAtUtc { get; init; }
    public required DateTime? EndsAtUtc { get; init; }
}
