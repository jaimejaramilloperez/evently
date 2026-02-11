using System.Data.Common;
using System.Text.Json;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Infrastructure.Inbox;
using MassTransit;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Evently.Modules.Events.Infrastructure.Inbox;

internal sealed class IntegrationEventConsumer<TIntegrationEvent>(
    IDbConnectionFactory dbConnectionFactory,
    IOptions<JsonOptions> jsonOptions)
    : IConsumer<TIntegrationEvent>
    where TIntegrationEvent : IntegrationEvent
{
    private readonly JsonOptions _jsonOptions = jsonOptions.Value;

    public async Task Consume(ConsumeContext<TIntegrationEvent> context)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        TIntegrationEvent integrationEvent = context.Message;

        InboxMessage inboxMessage = new()
        {
            Id = integrationEvent.Id,
            Type = integrationEvent.GetType().Name,
            Content = JsonSerializer.Serialize<IIntegrationEvent>(integrationEvent, _jsonOptions.SerializerOptions),
            OccurredAtUtc = integrationEvent.OccurredAtUtc,
        };

        const string sql =
            """
            INSERT INTO events.inbox_messages(id, type, content, occurred_at_utc)
            VALUES (@Id, @Type, @Content::json, @OccurredAtUtc)
            """;

        CommandDefinition command = new(
            commandText: sql,
            parameters: new
            {
                Id = inboxMessage.Id,
                Type = inboxMessage.Type,
                Content = inboxMessage.Content,
                OccurredAtUtc = inboxMessage.OccurredAtUtc,
            },
            cancellationToken: context.CancellationToken);

        await connection.ExecuteAsync(command);
    }
}
