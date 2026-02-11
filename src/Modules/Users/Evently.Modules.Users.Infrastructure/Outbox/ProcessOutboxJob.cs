using System.Data;
using System.Data.Common;
using System.Text.Json;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain.DomainEvents;
using Evently.Common.Infrastructure.Outbox;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Evently.Modules.Users.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<OutboxOptions> outboxOptions,
    IOptions<JsonOptions> jsonOptions,
    TimeProvider timeProvider,
    ILogger<ProcessOutboxJob> logger) : IJob
{
    private const string ModuleName = "users";
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;
    private readonly JsonOptions _jsonOptions = jsonOptions.Value;

    internal sealed record OutboxMessageResponse(Guid Id, string Content);

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Module {Module} - Beginning to process outbox messages", ModuleName);

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        using IServiceScope serviceScope = serviceScopeFactory.CreateScope();

        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                IDomainEvent? domainEvent = JsonSerializer.Deserialize<IDomainEvent>(
                    outboxMessage.Content,
                    _jsonOptions.SerializerOptions);

                if (domainEvent is null)
                {
                    throw new InvalidOperationException("Domain event is null");
                }

                IEnumerable<IDomainEventHandler> domainEventHandlers = DomainEventHandlersFactory.GetHandlers(
                    domainEvent.GetType(),
                    Application.AssemblyReference.Assembly,
                    serviceScope.ServiceProvider);

                foreach (IDomainEventHandler domainEventHandler in domainEventHandlers)
                {
                    await domainEventHandler.Handle(domainEvent, context.CancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Module {Module} - Exception while processing outbox message {MessageId}",
                    ModuleName,
                    outboxMessage.Id);

                exception = ex;
            }

            await UpdateOutboxMessagesAsync(connection, transaction, outboxMessage, exception);
        }

        await transaction.CommitAsync();

        logger.LogInformation("Module {Module} - Completed processing outbox messages", ModuleName);
    }

    private async Task<List<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        string sql =
            $"""
            SELECT
                id AS {nameof(OutboxMessageResponse.Id)},
                content AS {nameof(OutboxMessageResponse.Content)}
            FROM
                users.outbox_messages
            WHERE
                processed_at_utc IS NULL
            ORDER BY
                occurred_at_utc
            LIMIT
                {_outboxOptions.BatchSize}
            FOR UPDATE
            """;

        CommandDefinition command = new(
            commandText: sql,
            transaction: transaction);

        IEnumerable<OutboxMessageResponse> outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(command);

        return outboxMessages.AsList();
    }

    private async Task UpdateOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception = null)
    {
        const string sql =
            $"""
            UPDATE
                users.outbox_messages
            SET
                processed_at_utc = @ProcessedAtUtc,
                error = @Error
            WHERE
                id = @Id
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new
            {
                Id = outboxMessage.Id,
                ProcessedAtUtc = exception is null ? timeProvider.GetUtcNow().DateTime : (DateTime?)null,
                Error = exception?.ToString()
            },
            transaction: transaction);

        await connection.ExecuteAsync(command);
    }
}
