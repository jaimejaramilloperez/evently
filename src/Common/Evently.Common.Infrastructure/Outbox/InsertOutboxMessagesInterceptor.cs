using System.Text.Json;
using Evently.Common.Domain;
using Evently.Common.Domain.DomainEvents;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace Evently.Common.Infrastructure.Outbox;

public sealed class InsertOutboxMessagesInterceptor(IOptions<JsonOptions> jsonOptions)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context);
        }

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void InsertOutboxMessages(DbContext context)
    {
        List<OutboxMessage> outboxMessages = context.ChangeTracker.Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyCollection<IDomainEvent> domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = domainEvent.Id,
                Type = domainEvent.GetType().Name,
                Content = JsonSerializer.Serialize(domainEvent, jsonOptions.Value.SerializerOptions),
                OccurredAtUtc = domainEvent.OccurredAtUtc,
            })
            .ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}
