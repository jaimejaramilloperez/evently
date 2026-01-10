using Evently.Common.Application.Messaging;
using Evently.Modules.Events.Application.Events.GetEvents;

namespace Evently.Modules.Events.Application.Events.CreateEvent;

public sealed record CreateEventCommand : ICommand<EventResponse>
{
    public required Guid CategoryId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Location { get; init; }
    public required DateTime StartsAtUtc { get; init; }
    public DateTime? EndsAtUtc { get; init; }
}
