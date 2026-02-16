using System.Data.Common;

namespace Evently.Modules.Ticketing.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task<T> ExecuteWithinStrategyAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);
}
