using LinqKit;

namespace RST.Contracts;

/// <summary>
/// Represents an entity repository
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public interface IRepository<T> : IQueryable<T>
{
    /// <summary>
    /// Gets a value determining whether the entity should not be tracked
    /// </summary>
    bool NoTracking { set; }
    /// <summary>
    /// Adds the entity to the underlining provider
    /// </summary>
    /// <param name="entity"></param>
    void Add(T entity);
    /// <summary>
    /// Updates the entity to the underlining provider
    /// </summary>
    /// <param name="entity"></param>
    void Update(T entity);
    /// <summary>
    /// Gets the query builder
    /// </summary>
    ExpressionStarter<T> QueryBuilder { get; }
    /// <summary>
    /// Commits changes to underlining provider
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
    /// <returns>A awaiting <see cref="int"/> representing how many rows were affected</returns>
    Task<int> CommitChangesAsync(CancellationToken cancellationToken);
}
