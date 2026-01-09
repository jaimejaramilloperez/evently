using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Events.Infrastructure.TicketTypes;

internal sealed class TicketTypeDatabaseConfiguration : IEntityTypeConfiguration<TicketType>
{
    public void Configure(EntityTypeBuilder<TicketType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(x => x.EventId);

        builder.ToTable(x =>
        {
            x.HasCheckConstraint("ck_ticket_types_price", "price >= 0");
            x.HasCheckConstraint("ck_ticket_types_quantity", "quantity >= 0");
        });
    }
}
