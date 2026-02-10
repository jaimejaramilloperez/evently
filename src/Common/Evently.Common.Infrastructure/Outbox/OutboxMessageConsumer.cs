namespace Evently.Common.Infrastructure.Outbox;

public sealed class OutboxMessageConsumer(Guid outboxMessageId, string name)
{
    public Guid OutboxMessageId => outboxMessageId;
    public string Name => name;
}
