using Evently.Common.Application.Messaging;

namespace Evently.Modules.Ticketing.Application.Events.RescheduleEvent;

public sealed record RescheduleEventCommand : ICommand
{
    public required Guid EventId { get; init; }
    public required DateTime StartsAtUtc { get; init; }
    public required DateTime? EndsAtUtc { get; init; }
}
