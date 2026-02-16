using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Testcontainers.Keycloak;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Evently.IntegrationTests.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:18.1-alpine3.23")
        .WithDatabase("evently")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder("redis:8.4.0-alpine3.22")
        .WithCommand("redis-server", "--requirepass", "12345678")
        .Build();

    private readonly KeycloakContainer _keycloakContainer = new KeycloakBuilder("quay.io/keycloak/keycloak:26.5.2")
        .WithUsername("admin")
        .WithPassword("admin")
        .WithRealm("realm-export.json")
        .Build();

    public async ValueTask InitializeAsync()
    {
        await _dbContainer.StartAsync();
        await _redisContainer.StartAsync();
        await _keycloakContainer.StartAsync();
    }

    public new async ValueTask DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();

        await _redisContainer.StopAsync();
        await _redisContainer.DisposeAsync();

        await _keycloakContainer.StopAsync();
        await _keycloakContainer.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:Database", _dbContainer.GetConnectionString());
        builder.UseSetting("ConnectionStrings:Cache", _redisContainer.GetConnectionString());

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            string keyCloakAddress = _keycloakContainer.GetBaseAddress();
            string keyCloakRealmUrl = $"{keyCloakAddress}realms/evently";

            Dictionary<string, string?> configurationOverrides = new()
            {
                ["Authentication:TokenValidationParameters:ValidIssuers:1"] = keyCloakRealmUrl,
                ["Authentication:MetadataAddress"] = $"{keyCloakAddress}/.well-known/openid-configuration",
                ["Users:KeyCloak:AdminUrl"] = $"{keyCloakAddress}admin/realms/evently/",
                ["Users:KeyCloak:TokenUrl"] = $"{keyCloakRealmUrl}/protocol/openid-connect/token",
                ["Users:Outbox:IntervalInSeconds"] = "5",
                ["Users:Inbox:IntervalInSeconds"] = "5",
                ["Events:Outbox:IntervalInSeconds"] = "5",
                ["Events:Inbox:IntervalInSeconds"] = "5",
                ["Ticketing:Outbox:IntervalInSeconds"] = "5",
                ["Ticketing:Inbox:IntervalInSeconds"] = "5",
                ["Attendance:Outbox:IntervalInSeconds"] = "5",
                ["Attendance:Inbox:IntervalInSeconds"] = "5",
            };

            configBuilder.AddInMemoryCollection(configurationOverrides);
        });

        Quartz.Logging.LogContext.SetCurrentLogProvider(NullLoggerFactory.Instance);
    }
}
