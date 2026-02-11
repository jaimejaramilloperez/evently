using Evently.Modules.Attendance.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Attendance.Infrastructure.Events;

internal sealed class EventStatisticsDatabaseConfiguration : IEntityTypeConfiguration<EventStatistics>
{
    public void Configure(EntityTypeBuilder<EventStatistics> builder)
    {
        builder.ToTable("event_statistics");

        builder.HasKey(x => x.EventId);

        builder.Property(x => x.EventId)
            .ValueGeneratedNever();
    }
}
