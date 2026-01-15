using Evently.Common.Application.Caching;
using Evently.Common.Application.Data;
using Evently.Common.Infrastructure.Caching;
using Evently.Common.Infrastructure.Data;
using Evently.Common.Infrastructure.Interceptors;
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
        IConfiguration configuration)
    {
        string? databaseConnectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' was not found in configuration.");

        string? redisConnectionString = configuration.GetConnectionString("Cache")
            ?? throw new InvalidOperationException("Connection string 'Cache' was not found in configuration.");

        services.TryAddSingleton(new NpgsqlDataSourceBuilder(databaseConnectionString).Build());

        services.TryAddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.TryAddSingleton<PublishDomainEventsInterceptor>();

        services.AddSingleton(TimeProvider.System);

        IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);

        services.TryAddSingleton(connectionMultiplexer);

        services.AddStackExchangeRedisCache(options =>
        {
            options.ConnectionMultiplexerFactory = () => Task.FromResult(connectionMultiplexer);
        });

        services.TryAddSingleton<ICacheService, CacheService>();

        services.AddHealthChecks()
            .AddNpgSql(databaseConnectionString)
            .AddRedis(redisConnectionString);

        return services;
    }
}
