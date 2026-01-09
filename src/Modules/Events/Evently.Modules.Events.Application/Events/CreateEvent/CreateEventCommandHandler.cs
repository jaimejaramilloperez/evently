using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Events.GetEvents;
using Evently.Modules.Events.Domain.Abstractions.Results;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.CreateEvent;

internal sealed class CreateEventCommandHandler(
    TimeProvider timeProvider,
    IEventRepository eventRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEventCommand, EventResponse>
{
    public async Task<Result<EventResponse>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure<EventResponse>(CategoryErrors.NotFound(request.CategoryId));
        }

        Result<Event> result = Event.Create(
            timeProvider,
            category.Id,
            request.Title,
            request.Description,
            request.Location,
            request.StartsAtUtc,
            request.EndsAtUtc);

        if (result.IsFailure)
        {
            return Result.Failure<EventResponse>(result.Error);
        }

        eventRepository.Insert(result.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new EventResponse()
        {
            Id = result.Value.Id,
            CategoryId = result.Value.CategoryId,
            Title = result.Value.Title,
            Description = result.Value.Description,
            Location = result.Value.Location,
            StartsAtUtc = result.Value.StartsAtUtc,
            EndsAtUtc = result.Value.EndsAtUtc,
        };
    }
}
