using DotNet.Testcontainers.Containers;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Evently.IntegrationTests.Abstractions;

public static class SharedTestInfrastructure
{
    private static readonly Lazy<PostgreSqlContainer> _dbContainer = new(() =>
        new PostgreSqlBuilder("postgres:18.1-alpine3.23")
            .WithDatabase("evently")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build());

    private static readonly Lazy<RedisContainer> _redisContainer = new(() =>
        new RedisBuilder("redis:8.4.0-alpine3.22")
            .WithCommand("redis-server", "--requirepass", "12345678")
            .Build());

    private static readonly Lazy<KeycloakContainer> _keycloakContainer = new(() =>
        new KeycloakBuilder("quay.io/keycloak/keycloak:26.5.2")
            .WithUsername("admin")
            .WithPassword("admin")
            .WithRealm("realm-export.json")
            .Build());

    private static readonly Lazy<RabbitMqContainer> _rabbitMqContainer = new(() =>
        new RabbitMqBuilder("rabbitmq:4.2.4-management-alpine")
            .WithUsername("guest")
            .WithPassword("guest")
            .Build());

    public static PostgreSqlContainer Database => _dbContainer.Value;
    public static RedisContainer Redis => _redisContainer.Value;
    public static KeycloakContainer Keycloak => _keycloakContainer.Value;
    public static RabbitMqContainer RabbitMq => _rabbitMqContainer.Value;

    public static async Task StartAsync()
    {
        if (Database.State != TestcontainersStates.Running)
        {
            await Database.StartAsync();
        }

        if (Redis.State != TestcontainersStates.Running)
        {
            await Redis.StartAsync();
        }

        if (Keycloak.State != TestcontainersStates.Running)
        {
            await Keycloak.StartAsync();
        }

        if (RabbitMq.State != TestcontainersStates.Running)
        {
            await RabbitMq.StartAsync();
        }
    }

    public static async Task StopAsync()
    {
        if (Database.State == TestcontainersStates.Running)
        {
            await Database.StopAsync();
            await Database.DisposeAsync();
        }

        if (Redis.State == TestcontainersStates.Running)
        {
            await Redis.StopAsync();
            await Redis.DisposeAsync();
        }

        if (Keycloak.State == TestcontainersStates.Running)
        {
            await Keycloak.StopAsync();
            await Keycloak.DisposeAsync();
        }

        if (RabbitMq.State == TestcontainersStates.Running)
        {
            await RabbitMq.StopAsync();
            await RabbitMq.DisposeAsync();
        }
    }
}
