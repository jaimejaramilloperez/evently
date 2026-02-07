using Evently.Modules.Users.Domain.Roles;
using Evently.Modules.Users.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Users.Infrastructure.Roles;

internal sealed class RoleDatabaseConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Name);

        builder.Property(x => x.Name)
            .HasMaxLength(50);

        builder.HasMany<User>()
            .WithMany(x => x.Roles)
            .UsingEntity("user_roles", joinBuilder =>
            {
                joinBuilder.Property("RolesName")
                    .HasColumnName("role_name");
            });

        builder.HasData(
            Role.Member,
            Role.Administrator);
    }
}
