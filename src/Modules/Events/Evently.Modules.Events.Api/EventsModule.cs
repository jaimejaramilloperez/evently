using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Api.Events;
using Evently.Modules.Events.Api.Events.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Api;

public static class EventsModule
{
    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        string? databaseConnectionString = configuration.GetConnectionString("Database")
            ?? throw new InvalidOperationException("Connection string 'Database' was not found in configuration.");

        services.AddDbContext<EventsDbContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events);
                npgsqlOptions.EnableRetryOnFailure();
            });

            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }

    public static void MapEventsModuleEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder eventsGroup = app.MapGroup("/events")
            .WithTags(Tags.Events);

        GetEvent.MapEndpoint(eventsGroup);
        CreateEvent.MapEndpoint(eventsGroup);
    }
}
