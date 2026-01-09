using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Events.Infrastructure.Events;

internal sealed class EventDatabaseConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId);
    }
}
