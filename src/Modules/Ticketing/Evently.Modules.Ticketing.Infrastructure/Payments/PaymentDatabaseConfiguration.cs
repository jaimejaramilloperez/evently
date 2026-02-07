using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Ticketing.Infrastructure.Payments;

internal sealed class PaymentDatabaseConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne<Order>()
            .WithMany()
            .HasForeignKey(x => x.OrderId);

        builder.HasIndex(x => x.TransactionId)
            .IsUnique();
    }
}
