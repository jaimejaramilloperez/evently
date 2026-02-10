using Evently.Common.Infrastructure.Outbox;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Categories;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Infrastructure.Database;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Event> Events => Set<Event>();
    internal DbSet<Category> Categories => Set<Category>();
    internal DbSet<TicketType> TicketTypes => Set<TicketType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Events);

        modelBuilder.ApplyConfiguration(new OutboxMessageDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new EventDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new TicketTypeDatabaseConfiguration());
    }
}
