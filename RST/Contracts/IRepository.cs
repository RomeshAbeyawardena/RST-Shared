namespace RST.Contracts;

public interface IRepository<T> : IQueryable<T>
{
    Task<int> CommitChangesAsync(CancellationToken cancellationToken);
}
