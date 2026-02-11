namespace Evently.Modules.Attendance.Application.EventStatistics.GetEventStatistics;

public sealed record EventStatisticsResponse
{
    public required Guid EventId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string Location { get; init; }
    public required DateTime StartsAtUtc { get; init; }
    public required DateTime? EndsAtUtc { get; init; }
    public required int TicketsSold { get; init; }
    public required int AttendeesCheckedIn { get; init; }
    public required IReadOnlyCollection<string> DuplicateCheckInTickets { get; init; }
    public required IReadOnlyCollection<string> InvalidCheckInTickets { get; init; }
}
