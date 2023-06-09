﻿using LinqKit;
using RST.Contracts;
using System.Collections;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
namespace RST;

/// <summary>
/// Represents an entity base repository
/// </summary>
/// <typeparam name="T">Type of entity</typeparam>
public abstract class RepositoryBase<T> : IRepository<T>
{
    private IQueryable<T>? queryable;
    private ExpressionStarter<T>? queryBuilder;
    private readonly ISubject<ExpressionStarter<T>> subject;

    /// <summary>
    /// Sets the underlining <see cref="IQueryable"/>
    /// </summary>
    protected IQueryable<T> Queryable { set => queryable = value; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="queryable"></param>
    public RepositoryBase(ISubject<ExpressionStarter<T>> subject, IQueryable<T>? queryable = null)
    {
        this.subject = subject;
        ResetQueryBuilder();
        this.queryable = queryable;
    }

    /// <inheritdoc cref="IRepository{T}.Add(T)"/>
    public abstract void Add(T entity);

    /// <inheritdoc cref="IRepository{T}.Update(T)"/>
    public abstract void Update(T entity);

    /// <summary>
    /// Gets or sets a value determining whether entity tracking should be disabled
    /// </summary>
    public abstract bool NoTracking { set; }

    /// <summary>
    /// 
    /// </summary>
    public IObservable<ExpressionStarter<T>> OnReset => subject;

    /// <inheritdoc cref="IQueryable.ElementType" />
    public Type ElementType => queryable?.ElementType ?? throw new NullReferenceException();

    /// <inheritdoc cref="IQueryable.Expression" />
    public Expression Expression => queryable?.Expression ?? throw new NullReferenceException();

    /// <inheritdoc cref="IQueryable.Provider" />
    public IQueryProvider Provider => queryable?.Provider ?? throw new NullReferenceException();

    /// <inheritdoc cref="IRepository{T}.QueryBuilder"/>
    public ExpressionStarter<T> QueryBuilder => queryBuilder!;

    /// <inheritdoc cref="IRepository{T}.CommitChangesAsync(CancellationToken)"/>
    public abstract Task<int> CommitChangesAsync(CancellationToken cancellationToken);

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<T> GetEnumerator()
    {
        return (queryable ?? throw new NullReferenceException()).GetEnumerator();
    }


    /// <inheritdoc cref="IEnumerable.GetEnumerator" />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return (queryable ?? throw new NullReferenceException()).GetEnumerator();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    public abstract ValueTask<T?> FindAsync(CancellationToken cancellationToken, params object[] keys);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<IPagedResult<T>> GetPagedResult(Expression<Func<T, bool>> expression, IPagedQuery<int> query, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queryBuilder"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<IPagedResult<T>> GetPagedResult(Action<ExpressionStarter<T>> queryBuilder, IPagedQuery<int> query, CancellationToken cancellationToken);

    /// <summary>
    /// <inheritdoc cref="ResetQueryBuilder"/>
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void ResetQueryBuilder()
    {
        this.queryBuilder = PredicateBuilder.New<T>();
        subject.OnNext(queryBuilder);
    }
}
