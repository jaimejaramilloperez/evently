using Evently.Common.Application.Caching;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Infrastructure.Authentication;
using Evently.Common.Infrastructure.Caching;
using Evently.Common.Infrastructure.Data;
using Evently.Common.Infrastructure.Interceptors;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;

namespace Evently.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IRegistrationConfigurator>[] moduleConfigureConsumers)
    {
        string databaseConnectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' was not found in configuration.");

        string redisConnectionString = configuration.GetConnectionString("Cache")
            ?? throw new InvalidOperationException("Connection string 'Cache' was not found in configuration.");

        services.TryAddSingleton(new NpgsqlDataSourceBuilder(databaseConnectionString).Build());

        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.TryAddSingleton<PublishDomainEventsInterceptor>();

        services.AddSingleton(TimeProvider.System);

        services.AddHttpContextAccessor();

        services.AddAuthenticationInternal();

        try
        {
            IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);

            services.TryAddSingleton(connectionMultiplexer);

            services.AddStackExchangeRedisCache(options =>
            {
                options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
            });
        }
        catch
        {

            services.AddDistributedMemoryCache();
        }

        services.TryAddSingleton<ICacheService, CacheService>();

        services.TryAddSingleton<IEventBus, EventBus.EventBus>();

        services.AddMassTransit(options =>
        {
            options.SetKebabCaseEndpointNameFormatter();

            foreach (Action<IRegistrationConfigurator> configureConsumer in moduleConfigureConsumers)
            {
                configureConsumer(options);
            }

            options.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
            });
        });

        string keyCloakHealthUrl = configuration.GetSection("KeyCloak").GetValue<string>("HealthUrl")
            ?? throw new InvalidOperationException("Configuration value 'KeyCloak__HealthUrl' was not found in configuration.");

        services.AddHealthChecks()
            .AddNpgSql(databaseConnectionString)
            .AddRedis(redisConnectionString)
            .AddUrlGroup(new Uri(keyCloakHealthUrl), HttpMethod.Get, "keycloak");

        return services;
    }
}
