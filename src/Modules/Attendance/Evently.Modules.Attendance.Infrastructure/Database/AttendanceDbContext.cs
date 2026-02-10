using Evently.Common.Infrastructure.Outbox;
using Evently.Modules.Attendance.Application.Abstractions.Data;
using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Evently.Modules.Attendance.Infrastructure.Attendees;
using Evently.Modules.Attendance.Infrastructure.Events;
using Evently.Modules.Attendance.Infrastructure.Tickets;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Attendance.Infrastructure.Database;

public sealed class AttendanceDbContext(DbContextOptions<AttendanceDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Attendee> Attendees => Set<Attendee>();
    internal DbSet<Event> Events => Set<Event>();
    internal DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Attendance);

        modelBuilder.ApplyConfiguration(new OutboxMessageDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new AttendeeDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new EventDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new TicketDatabaseConfiguration());
    }
}
