using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.DomainEvents;
using Evently.Common.Infrastructure.Outbox;

namespace Evently.Modules.Attendance.Infrastructure.Outbox;

internal sealed class IdempotentDomainEventHandler<TDomainEvent>(
    IDomainEventHandler<TDomainEvent> decorated,
    IDbConnectionFactory dbConnectionFactory)
    : DomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        OutboxMessageConsumer outboxMessageConsumer = new(domainEvent.Id, decorated.GetType().Name);

        bool consumerExists = await OutboxConsumerExistsAsync(connection, outboxMessageConsumer, cancellationToken);

        if (consumerExists) return;

        await decorated.Handle(domainEvent, cancellationToken);

        await InsertOutboxConsumerAsync(connection, outboxMessageConsumer, cancellationToken);
    }

    private static async Task<bool> OutboxConsumerExistsAsync(
        DbConnection dbConnection,
        OutboxMessageConsumer outboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            SELECT EXISTS(
                SELECT
                    1
                FROM
                    attendance.outbox_message_consumers
                WHERE
                    outbox_message_id = @OutboxMessageId
                    AND
                    name = @Name
            )
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new { Name = outboxMessageConsumer.Name },
            cancellationToken: cancellationToken);

        return await dbConnection.ExecuteScalarAsync<bool>(command);
    }

    private static async Task InsertOutboxConsumerAsync(
        DbConnection dbConnection,
        OutboxMessageConsumer outboxMessageConsumer,
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            INSERT INTO attendance.outbox_message_consumers(outbox_message_id, name)
            VALUES (@OutboxMessageId, @Name)
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new
            {
                OutboxMessageId = outboxMessageConsumer.OutboxMessageId,
                Name = outboxMessageConsumer.Name
            },
            cancellationToken: cancellationToken);

        await dbConnection.ExecuteAsync(command);
    }
}
