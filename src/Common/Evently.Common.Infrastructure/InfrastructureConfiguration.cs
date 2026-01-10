using Evently.Common.Application.Data;
using Evently.Common.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Evently.Common.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string? databaseConnectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' was not found in configuration.");

        services.TryAddSingleton(new NpgsqlDataSourceBuilder(databaseConnectionString).Build());

        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
