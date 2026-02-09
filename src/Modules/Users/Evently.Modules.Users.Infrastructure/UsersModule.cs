using Evently.Common.Application.Authorization;
using Evently.Common.Infrastructure.Configuration;
using Evently.Common.Infrastructure.Interceptors;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Application.Abstractions.Identity;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure.Authorization;
using Evently.Modules.Users.Infrastructure.Database;
using Evently.Modules.Users.Infrastructure.Identity;
using Evently.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Evently.Modules.Users.Infrastructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        services.AddInfrastructure(configuration);

        return services;
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string databaseConnectionString = configuration.GetConnectionStringOrThrow("Database");

        services.AddDbContext<UsersDbContext>((sp, options) =>
        {
            options.UseNpgsql(databaseConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Users);
                npgsqlOptions.EnableRetryOnFailure();
            });

            options.UseSnakeCaseNamingConvention();
            options.AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>());
        });

        services.AddScoped<IPermissionService, PermissionService>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());

        services.Configure<KeyCloakOptions>(configuration.GetSection("Users").GetSection("KeyCloak"));

        services.AddTransient<KeyCloakAuthDelegatingHandler>();

        services.AddHttpClient<KeyCloakClient>((sp, httpClient) =>
        {
            KeyCloakOptions keyCloakOptions = sp.GetRequiredService<IOptions<KeyCloakOptions>>().Value;
            httpClient.BaseAddress = new Uri(keyCloakOptions.AdminUrl);
        })
        .AddHttpMessageHandler<KeyCloakAuthDelegatingHandler>();

        services.AddTransient<IIdentityProviderService, IdentityProviderService>();
    }
}
