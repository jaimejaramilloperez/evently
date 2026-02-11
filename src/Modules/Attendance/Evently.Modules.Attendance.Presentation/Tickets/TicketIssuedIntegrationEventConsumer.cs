using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Domain.Results;
using Evently.Modules.Attendance.Application.Tickets.CreateTicket;
using Evently.Modules.Ticketing.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Attendance.Presentation.Tickets;

public sealed class TicketIssuedIntegrationEventConsumer(ISender sender)
    : IntegrationEventHandler<TicketIssuedIntegrationEvent>
{
    public override async Task Handle(
        TicketIssuedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        CreateTicketCommand command = new()
        {
            TicketId = integrationEvent.TicketId,
            AttendeeId = integrationEvent.CustomerId,
            EventId = integrationEvent.EventId,
            Code = integrationEvent.Code,
        };

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CreateTicketCommand), result.Error);
        }
    }
}
