using Evently.Modules.Events.Application.Events.GetEvents;

namespace Evently.Modules.Events.Application.Events.SearchEvents;

public sealed record SearchEventsResponse
{
    public required int Page { get; init; }
    public required int PageSize { get; init; }
    public required int TotalCount { get; init; }
    public IReadOnlyCollection<EventResponse> Events { get; init; } = [];
}
