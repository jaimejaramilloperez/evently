using Evently.Common.Application.Messaging;

namespace Evently.Modules.Attendance.Application.Tickets.CreateTicket;

public sealed record CreateTicketCommand : ICommand
{
    public required Guid TicketId { get; init; }
    public required Guid AttendeeId { get; init; }
    public required Guid EventId { get; init; }
    public required string Code { get; init; }
}
