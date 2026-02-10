using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Common.Infrastructure.Outbox;

public sealed class OutboxMessageConsumerDatabaseConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        builder.ToTable("outbox_message_consumers");

        builder.HasKey(x => new { x.OutboxMessageId, x.Name });

        builder.Property(x => x.Name)
            .HasMaxLength(500);
    }
}
