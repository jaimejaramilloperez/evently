using Evently.Common.Application.Exceptions;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Ticketing.Application.Tickets.GetTicket;
using Evently.Modules.Ticketing.Application.Tickets.GetTicketForOrder;
using Evently.Modules.Ticketing.Domain.Orders.DomainEvents;
using MediatR;

namespace Evently.Modules.Ticketing.Application.Tickets.CreateTicketBatch;

internal sealed class OrderTicketsIssuedDomainEventHandler(ISender sender)
    : DomainEventHandler<OrderTicketsIssuedDomainEvent>
{
    public override async Task Handle(
        OrderTicketsIssuedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        GetTicketsForOrderQuery query = new(domainEvent.OrderId);

        Result<IReadOnlyCollection<TicketResponse>> result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(GetTicketsForOrderQuery), result.Error);
        }

        // Send ticket confirmation notification.
    }
}
