using System.Data.Common;
using Evently.Modules.Ticketing.Application.Abstractions.Data;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Domain.Events;
using Evently.Modules.Ticketing.Domain.Orders;
using Evently.Modules.Ticketing.Domain.Payments;
using Evently.Modules.Ticketing.Domain.Tickets;
using Evently.Modules.Ticketing.Infrastructure.Customers;
using Evently.Modules.Ticketing.Infrastructure.Events;
using Evently.Modules.Ticketing.Infrastructure.Orders;
using Evently.Modules.Ticketing.Infrastructure.Payments;
using Evently.Modules.Ticketing.Infrastructure.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Evently.Modules.Ticketing.Infrastructure.Database;

public sealed class TicketingDbContext(DbContextOptions<TicketingDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Customer> Customers => Set<Customer>();
    internal DbSet<Event> Events => Set<Event>();
    internal DbSet<TicketType> TicketTypes => Set<TicketType>();
    internal DbSet<Order> Orders => Set<Order>();
    internal DbSet<OrderItem> OrderItems => Set<OrderItem>();
    internal DbSet<Ticket> Tickets => Set<Ticket>();
    internal DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Ticketing);

        modelBuilder.ApplyConfiguration(new CustomerDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new EventDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new TicketTypeDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new TicketDatabaseConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentDatabaseConfiguration());
    }

    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
        {
            await Database.CurrentTransaction.DisposeAsync();
        }

        IDbContextTransaction dbContextTransaction = await Database.BeginTransactionAsync(cancellationToken);

        return dbContextTransaction.GetDbTransaction();
    }
}
