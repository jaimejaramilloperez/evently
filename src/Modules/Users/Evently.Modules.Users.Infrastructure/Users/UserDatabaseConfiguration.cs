using Evently.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Users.Infrastructure.Users;

internal sealed class UserDatabaseConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasDefaultValueSql("uuidv7()")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.IdentityId)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasMaxLength(200);

        builder.Property(x => x.LastName)
            .HasMaxLength(200);

        builder.Property(x => x.Email)
            .HasMaxLength(300);

        builder.HasIndex(x => x.IdentityId)
            .IsUnique();

        builder.HasIndex(x => x.Email)
            .IsUnique();
    }
}
