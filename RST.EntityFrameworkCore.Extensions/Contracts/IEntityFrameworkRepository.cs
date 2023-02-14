using RST.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LinqKit;

namespace RST.Persistence.Contracts;

/// <summary>
/// Represents an Entity framework repository
/// </summary>
/// <typeparam name="TDbContext">The Db Context type to be used by this repository</typeparam>
/// <typeparam name="T">The entity type for the underlining repository</typeparam>
public interface IEntityFrameworkRepository<TDbContext, T> : IRepository<T>
    where TDbContext : DbContext
    where T : class
{
    /// <summary>
    /// Gets an instance of <typeparamref name="TDbContext"/> as an instance of <see cref="DbContext"/>
    /// </summary>
    TDbContext Context { get; }

    /// <summary>
    /// Get paged result
    /// </summary>
    /// <param name="expression">Expression to query</param>
    /// <param name="query">Instance of paged query used for filtering</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IPagedResult<int, T>> GetPagedResult(Expression<Func<T, bool>> expression, IPagedQuery<int> query, CancellationToken cancellationToken);

    /// <summary>
    /// Get paged result
    /// </summary>
    /// <param name="queryBuilder">Query builder to create an expression from</param>
    /// <param name="query">Instance of paged query used for filtering</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IPagedResult<int, T>> GetPagedResult(Action<ExpressionStarter<T>> queryBuilder, IPagedQuery<int> query, CancellationToken cancellationToken);
} 