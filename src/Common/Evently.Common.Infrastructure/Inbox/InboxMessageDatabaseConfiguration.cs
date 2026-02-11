using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Common.Infrastructure.Inbox;

public sealed class InboxMessageDatabaseConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("inbox_messages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Content)
            .HasColumnType("jsonb");
    }
}
