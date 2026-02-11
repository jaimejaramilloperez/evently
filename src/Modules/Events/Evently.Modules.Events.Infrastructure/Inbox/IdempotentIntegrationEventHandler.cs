using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Infrastructure.Inbox;

namespace Evently.Modules.Events.Infrastructure.Inbox;

internal sealed class IdempotentIntegrationEventHandler<TIntegrationEvent>(
    IIntegrationEventHandler<TIntegrationEvent> decorated,
    IDbConnectionFactory dbConnectionFactory)
    : IntegrationEventHandler<TIntegrationEvent>
    where TIntegrationEvent : IIntegrationEvent
{
    public override async Task Handle(
        TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        InboxMessageConsumer inboxMessageConsumer = new(integrationEvent.Id, decorated.GetType().Name);

        bool consumerExists = await InboxConsumerExistsAsync(connection, inboxMessageConsumer, cancellationToken);

        if (consumerExists) return;

        await decorated.Handle(integrationEvent, cancellationToken);

        await InsertInboxConsumerAsync(connection, inboxMessageConsumer, cancellationToken);
    }

    private static async Task<bool> InboxConsumerExistsAsync(
        DbConnection dbConnection,
        InboxMessageConsumer inboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT EXISTS(
                SELECT
                    1
                FROM
                    events.inbox_message_consumers
                WHERE
                    inbox_message_id = @InboxMessageId
                    AND
                    name = @Name
            )
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new
            {
                InboxMessageId = inboxMessageConsumer.InboxMessageId,
                Name = inboxMessageConsumer.Name,
            },
            cancellationToken: cancellationToken);

        return await dbConnection.ExecuteScalarAsync<bool>(command);
    }

    private static async Task InsertInboxConsumerAsync(
        DbConnection dbConnection,
        InboxMessageConsumer inboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            INSERT INTO events.inbox_message_consumers(inbox_message_id, name)
            VALUES (@InboxMessageId, @Name)
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new
            {
                InboxMessageId = inboxMessageConsumer.InboxMessageId,
                Name = inboxMessageConsumer.Name,
            },
            cancellationToken: cancellationToken);

        await dbConnection.ExecuteAsync(command);
    }
}
