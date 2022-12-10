using LinqKit;

namespace RST.Contracts;

public interface IRepository<T> : IQueryable<T>
{
    bool NoTracking { set; }
    void Add(T entity);
    void Update(T entity);
    ExpressionStarter<T> QueryBuilder { get; }
    Task<int> CommitChangesAsync(CancellationToken cancellationToken);
}
