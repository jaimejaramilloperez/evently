using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Ticketing.Infrastructure.Tickets;

internal sealed class TicketDatabaseConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId);

        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId);

        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(x => x.EventId);

        builder.HasOne<TicketType>()
            .WithMany()
            .HasForeignKey(x => x.TicketTypeId);
    }
}
