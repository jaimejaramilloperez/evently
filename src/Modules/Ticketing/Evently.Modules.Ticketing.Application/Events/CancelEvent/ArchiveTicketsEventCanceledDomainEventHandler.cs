using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Tickets.ArchiveTicketsForEvent;
using Evently.Modules.Ticketing.Domain.Events.DomainEvents;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Events.CancelEvent;

internal sealed class ArchiveTicketsEventCanceledDomainEventHandler(ISender sender)
    : IDomainEventHandler<EventCanceledDomainEvent>
{
    public async Task Handle(EventCanceledDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        ArchiveTicketsForEventCommand command = new(domainEvent.EventId);

        Result result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(ArchiveTicketsForEventCommand), result.Error);
        }
    }
}
