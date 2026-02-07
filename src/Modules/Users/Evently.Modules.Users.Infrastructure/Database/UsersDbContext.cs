using Evently.Modules.Users.Application.Abstractions.Data;
using Evently.Modules.Users.Domain.Permissions;
using Evently.Modules.Users.Domain.Roles;
using Evently.Modules.Users.Domain.Users;
using Evently.Modules.Users.Infrastructure.Permissions;
using Evently.Modules.Users.Infrastructure.Roles;
using Evently.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Users.Infrastructure.Database;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<User> Users => Set<User>();
    internal DbSet<Role> Roles => Set<Role>();
    internal DbSet<Permission> Permissions => Set<Permission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfiguration(new UserDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new RoleDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionDatabaseConfiguration());
    }
}
