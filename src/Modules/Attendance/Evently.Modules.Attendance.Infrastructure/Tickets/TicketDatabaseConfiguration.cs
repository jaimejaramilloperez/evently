using Evently.Modules.Attendance.Domain.Attendees;
using Evently.Modules.Attendance.Domain.Events;
using Evently.Modules.Attendance.Domain.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Attendance.Infrastructure.Tickets;

internal sealed class TicketDatabaseConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasOne<Attendee>()
            .WithMany()
            .HasForeignKey(x => x.AttendeeId);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(x => x.EventId);
    }
}
