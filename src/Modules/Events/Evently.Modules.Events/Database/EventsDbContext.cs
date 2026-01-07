using Evently.Modules.Events.Events.Models;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Database;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options)
    : DbContext(options)
{
    internal DbSet<Event> Events => Set<Event>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Events);

        modelBuilder.Entity<Event>().HasKey(x => x.Id);

        modelBuilder.Entity<Event>().Property(x => x.Id)
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();
    }
}
