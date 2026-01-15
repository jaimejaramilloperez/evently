using Evently.Common.Domain;
using Evently.Common.Domain.DomainEvents;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Common.Infrastructure.Interceptors;

public sealed class PublishDomainEventsInterceptor(IServiceScopeFactory serviceScopeFactory)
    : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            await PublishDomainEventsAsync(eventData.Context);
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEventsAsync(DbContext context)
    {
        IEnumerable<IDomainEvent> domainEvents = context.ChangeTracker.Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            });

        using IServiceScope scope = serviceScopeFactory.CreateScope();

        IPublisher publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}
