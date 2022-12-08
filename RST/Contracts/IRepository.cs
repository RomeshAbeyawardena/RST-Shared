using LinqKit;

namespace RST.Contracts;

public interface IRepository<T> : IQueryable<T>
{
    ExpressionStarter<T> QueryBuilder { get; }
    Task<int> CommitChangesAsync(CancellationToken cancellationToken);
}
