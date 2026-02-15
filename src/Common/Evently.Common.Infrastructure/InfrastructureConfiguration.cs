using Evently.Common.Application.Caching;
using Evently.Common.Application.Data;
using Evently.Common.Application.EventBus;
using Evently.Common.Infrastructure.Authentication;
using Evently.Common.Infrastructure.Authorization;
using Evently.Common.Infrastructure.Caching;
using Evently.Common.Infrastructure.Configuration;
using Evently.Common.Infrastructure.Data;
using Evently.Common.Infrastructure.Outbox;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Quartz;
using StackExchange.Redis;

namespace Evently.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IRegistrationConfigurator>[] moduleConfigureConsumers)
    {
        string databaseConnectionString = configuration.GetConnectionStringOrThrow("Database");
        string redisConnectionString = configuration.GetConnectionStringOrThrow("Cache");

        services.TryAddSingleton(new NpgsqlDataSourceBuilder(databaseConnectionString).Build());

        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();

        services.AddSingleton(TimeProvider.System);

        services.AddHttpContextAccessor();

        services.AddAuthenticationInternal();

        services.AddAuthorizationInternal();

        services.AddQuartz(options =>
        {
            Guid scheduler = Guid.NewGuid();
            options.SchedulerId = $"default-id-{scheduler}";
            options.SchedulerName = $"default-name-{scheduler}";
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

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

        return services;
    }
}
