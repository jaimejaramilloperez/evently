namespace Evently.Common.Infrastructure.Inbox;

public sealed class InboxMessageConsumer(Guid inboxMessageId, string name)
{
    public Guid InboxMessageId => inboxMessageId;
    public string Name => name;
}
