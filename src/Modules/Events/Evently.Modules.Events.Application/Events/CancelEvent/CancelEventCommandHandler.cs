using Evently.Common.Application.Messaging;
using Evently.Common.Domain.Results;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.CancelEvent;

internal sealed class CancelEventCommandHandler(
    TimeProvider timeProvider,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelEventCommand>
{
    public async Task<Result> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        Event? @event = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (@event is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }

        Result result = @event.Cancel(timeProvider);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
