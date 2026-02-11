using System.Data;
using System.Data.Common;
using System.Text.Json;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Infrastructure.Inbox;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Evently.Modules.Events.Infrastructure.Inbox;

[DisallowConcurrentExecution]
internal sealed class ProcessInboxJob(
    IDbConnectionFactory dbConnectionFactory,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<InboxOptions> inboxOptions,
    IOptions<JsonOptions> jsonOptions,
    TimeProvider timeProvider,
    ILogger<ProcessInboxJob> logger) : IJob
{
    private const string ModuleName = "events";
    private readonly InboxOptions _inboxOptions = inboxOptions.Value;
    private readonly JsonOptions _jsonOptions = jsonOptions.Value;

    internal sealed record InboxMessageResponse(Guid Id, string Content);

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Module {Module} - Beginning to process inbox messages", ModuleName);

        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();
        await using DbTransaction transaction = await connection.BeginTransactionAsync();

        IReadOnlyList<InboxMessageResponse> inboxMessages = await GetInboxMessagesAsync(connection, transaction);

        using IServiceScope serviceScope = serviceScopeFactory.CreateScope();

        foreach (InboxMessageResponse inboxMessage in inboxMessages)
        {
            Exception? exception = null;

            try
            {
                IIntegrationEvent? integrationEvent = JsonSerializer.Deserialize<IIntegrationEvent>(
                    inboxMessage.Content,
                    _jsonOptions.SerializerOptions);

                if (integrationEvent is null)
                {
                    throw new InvalidOperationException("Integration event is null");
                }

                IEnumerable<IIntegrationEventHandler> integrationEventHandlers = IntegrationEventHandlersFactory.GetHandlers(
                    integrationEvent.GetType(),
                    serviceScope.ServiceProvider,
                    Presentation.AssemblyReference.Assembly);

                foreach (IIntegrationEventHandler integrationEventHandler in integrationEventHandlers)
                {
                    await integrationEventHandler.Handle(integrationEvent, context.CancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Module {Module} - Exception while processing outbox message {MessageId}",
                    ModuleName,
                    inboxMessage.Id);

                exception = ex;
            }

            await UpdateInboxMessageAsync(connection, transaction, inboxMessage, exception);
        }

        await transaction.CommitAsync();

        logger.LogInformation("Module {Module} - Completed processing inbox messages", ModuleName);
    }

    private async Task<List<InboxMessageResponse>> GetInboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        string sql =
            $"""
            SELECT
                id AS {nameof(InboxMessageResponse.Id)},
                content AS {nameof(InboxMessageResponse.Content)}
            FROM
                events.inbox_messages
            WHERE
                processed_at_utc IS NULL
            ORDER BY
                occurred_at_utc
            LIMIT
                {_inboxOptions.BatchSize}
            FOR UPDATE
            """;

        CommandDefinition command = new(
            commandText: sql,
            transaction: transaction);

        IEnumerable<InboxMessageResponse> inboxMessages = await connection.QueryAsync<InboxMessageResponse>(command);

        return inboxMessages.AsList();
    }

    private async Task UpdateInboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        InboxMessageResponse inboxMessage,
        Exception? exception)
    {
        const string sql =
            """
            UPDATE
                events.inbox_messages
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
                Id = inboxMessage.Id,
                ProcessedAtUtc = exception is null ? timeProvider.GetUtcNow().DateTime : (DateTime?)null,
                Error = exception?.ToString()
            },
            transaction: transaction);

        await connection.ExecuteAsync(command);
    }
}
