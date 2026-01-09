using Evently.Modules.Events.Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Events.Infrastructure.Categories;

internal sealed class CategoryDatabaseConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.IsArchived)
            .HasDefaultValue(false);
    }
}
