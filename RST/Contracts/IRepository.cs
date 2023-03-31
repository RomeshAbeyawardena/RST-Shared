﻿using LinqKit;
using System.Linq.Expressions;

namespace RST.Contracts;

/// <summary>
/// Represents an entity repository
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public interface IRepository<T> : IQueryable<T>
{
    /// <summary>
    /// 
    /// </summary>
    IObservable<ExpressionStarter<T>> OnReset { get; }
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
    /// Resets the query builder instance
    /// </summary>
    void ResetQueryBuilder();

    /// <summary>
    /// Commits changes to underlining provider
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
    /// <returns>A awaiting <see cref="int"/> representing how many rows were affected</returns>
    Task<int> CommitChangesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Finds an entity from the underlining data store
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    ValueTask<T?> FindAsync(CancellationToken cancellationToken, params object[] keys);

    /// <summary>
    /// Get paged result
    /// </summary>
    /// <param name="expression">Expression to query</param>
    /// <param name="query">Instance of paged query used for filtering</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IPagedResult<T>> GetPagedResult(Expression<Func<T, bool>> expression, IPagedQuery<int> query, CancellationToken cancellationToken);

    /// <summary>
    /// Get paged result
    /// </summary>
    /// <param name="queryBuilder">Query builder to create an expression from</param>
    /// <param name="query">Instance of paged query used for filtering</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IPagedResult<T>> GetPagedResult(Action<ExpressionStarter<T>> queryBuilder, IPagedQuery<int> query, CancellationToken cancellationToken);
}
