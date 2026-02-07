using Evently.Modules.Users.Domain.Permissions;
using Evently.Modules.Users.Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evently.Modules.Users.Infrastructure.Permissions;

internal sealed class PermissionDatabaseConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(x => x.Code);

        builder.Property(x => x.Code)
            .HasMaxLength(100);

        builder.HasData(
            Permission.GetUser,
            Permission.ModifyUser,
            Permission.GetEvents,
            Permission.SearchEvents,
            Permission.ModifyEvents,
            Permission.GetTicketTypes,
            Permission.ModifyTicketTypes,
            Permission.GetCategories,
            Permission.ModifyCategories,
            Permission.GetCart,
            Permission.AddToCart,
            Permission.RemoveFromCart,
            Permission.GetOrders,
            Permission.CreateOrder,
            Permission.GetTickets,
            Permission.CheckInTicket,
            Permission.GetEventStatistics);

        builder.HasMany<Role>()
            .WithMany()
            .UsingEntity("role_permissions", joinBuilder =>
            {
                joinBuilder.HasData(
                    // Member permissions
                    CreateRolePermission(Role.Member, Permission.GetUser),
                    CreateRolePermission(Role.Member, Permission.ModifyUser),
                    CreateRolePermission(Role.Member, Permission.SearchEvents),
                    CreateRolePermission(Role.Member, Permission.GetTicketTypes),
                    CreateRolePermission(Role.Member, Permission.GetCart),
                    CreateRolePermission(Role.Member, Permission.AddToCart),
                    CreateRolePermission(Role.Member, Permission.RemoveFromCart),
                    CreateRolePermission(Role.Member, Permission.GetOrders),
                    CreateRolePermission(Role.Member, Permission.CreateOrder),
                    CreateRolePermission(Role.Member, Permission.GetTickets),
                    CreateRolePermission(Role.Member, Permission.CheckInTicket),
                    // Admin permissions
                    CreateRolePermission(Role.Administrator, Permission.GetUser),
                    CreateRolePermission(Role.Administrator, Permission.ModifyUser),
                    CreateRolePermission(Role.Administrator, Permission.GetEvents),
                    CreateRolePermission(Role.Administrator, Permission.SearchEvents),
                    CreateRolePermission(Role.Administrator, Permission.ModifyEvents),
                    CreateRolePermission(Role.Administrator, Permission.GetTicketTypes),
                    CreateRolePermission(Role.Administrator, Permission.ModifyTicketTypes),
                    CreateRolePermission(Role.Administrator, Permission.GetCategories),
                    CreateRolePermission(Role.Administrator, Permission.ModifyCategories),
                    CreateRolePermission(Role.Administrator, Permission.GetCart),
                    CreateRolePermission(Role.Administrator, Permission.AddToCart),
                    CreateRolePermission(Role.Administrator, Permission.RemoveFromCart),
                    CreateRolePermission(Role.Administrator, Permission.GetOrders),
                    CreateRolePermission(Role.Administrator, Permission.CreateOrder),
                    CreateRolePermission(Role.Administrator, Permission.GetTickets),
                    CreateRolePermission(Role.Administrator, Permission.CheckInTicket),
                    CreateRolePermission(Role.Administrator, Permission.GetEventStatistics));
            });
    }

    private static object CreateRolePermission(Role role, Permission permission)
    {
        return new
        {
            RoleName = role.Name,
            PermissionCode = permission.Code,
        };
    }
}
