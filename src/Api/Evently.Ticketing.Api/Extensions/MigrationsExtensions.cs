using Evently.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Ticketing.Api.Extensions;

public static class MigrationsExtensions
{
    internal static void ApplyMigrations(this WebApplication app)
    {
        app.ApplyMigration<TicketingDbContext>();
    }

    private static void ApplyMigration<TDbContext>(this WebApplication app)
        where TDbContext : DbContext
    {
        using IServiceScope scope = app.Services.CreateScope();
        using TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        try
        {
            dbContext.Database.Migrate();
            app.Logger.LogInformation("Database migrations for {DbContext} applied successfully", typeof(TDbContext).Name);
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while applying database migrations for {DbContext}", typeof(TDbContext).Name);
            throw;
        }
    }
}
