using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.TicketTypes.GetTicketType;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;

namespace Evently.Modules.Events.Application.TicketTypes.CreateTicketType;

internal sealed class CreateTicketTypeCommandHandler(
    IEventRepository eventRepository,
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateTicketTypeCommand, TicketTypeResponse>
{
    public async Task<Result<TicketTypeResponse>> Handle(CreateTicketTypeCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure<TicketTypeResponse>(EventErrors.NotFound(request.EventId));
        }

        TicketType ticketType = TicketType.Create(
            @event.Id,
            request.Name,
            request.Price,
            request.Currency,
            request.Quantity);

        ticketTypeRepository.Insert(ticketType);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new TicketTypeResponse()
        {
            Id = ticketType.Id,
            EventId = ticketType.EventId,
            Name = ticketType.Name,
            Price = ticketType.Price,
            Currency = ticketType.Currency,
            Quantity = ticketType.Quantity,
        };
    }
}
