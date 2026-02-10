using Evently.Common.Application.EventBus;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Tickets.GetTicket;
using Evently.Modules.Ticketing.Domain.Tickets.DomainEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

internal sealed class TicketCreatedDomainEventHandler(ISender sender, IEventBus eventBus)
    : DomainEventHandler<TicketCreatedDomainEvent>
{
    public override async Task Handle(
        TicketCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        GetTicketQuery query = new(domainEvent.TicketId);

        Result<TicketResponse> result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(GetTicketQuery), result.Error);
        }

        TicketIssuedIntegrationEvent integrationEvent = new(
            domainEvent.Id,
            domainEvent.OccurredAtUtc,
            result.Value.Id,
            result.Value.CustomerId,
            result.Value.EventId,
            result.Value.Code);

        await eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
