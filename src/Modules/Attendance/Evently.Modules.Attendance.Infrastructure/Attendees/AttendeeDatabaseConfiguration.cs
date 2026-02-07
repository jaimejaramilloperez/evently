using Evently.Modules.Attendance.Domain.Attendees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Attendance.Infrastructure.Attendees;

internal sealed class AttendeeDatabaseConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName)
            .HasMaxLength(200);

        builder.Property(x => x.LastName)
            .HasMaxLength(200);

        builder.Property(x => x.Email)
            .HasMaxLength(300);
    }
}
