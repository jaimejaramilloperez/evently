using Evently.Modules.Events.Application.Abstractions.Messaging;

namespace Evently.Modules.Events.Application.Events.SearchEvents;

public sealed record SearchEventsQuery : IQuery<SearchEventsResponse>
{
    public Guid? CategoryId { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
}
