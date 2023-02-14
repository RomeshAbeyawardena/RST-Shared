using RST.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using RST.Contracts;
using RST.Defaults;
using System.Linq.Expressions;
using LinqKit;
using System.Linq.Dynamic.Core;

namespace RST.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <typeparam name="T"></typeparam>
public abstract class EntityFrameworkRepositoryBase<TDbContext, T> : RepositoryBase<T>, IEntityFrameworkRepository<TDbContext, T>
    where TDbContext : DbContext
    where T : class
{
    private readonly DbSet<T> dbSet;

    private void ConfigureTracking(bool noTracking)
    {
        Queryable = noTracking ? dbSet.AsNoTracking() : dbSet;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public EntityFrameworkRepositoryBase(TDbContext context)
        : base()
    {
        dbSet = context.Set<T>();
        Queryable = dbSet.AsNoTracking();
        Context = context;
    }

    /// <inheritdoc cref="Contracts.IRepository{T}.Add(T)"/>
    public override void Add(T entity)
    {
        dbSet.Add(entity);
    }

    /// <inheritdoc cref="Contracts.IRepository{T}.Update(T)"/>
    public override void Update(T entity)
    {
        dbSet.Update(entity);
    }

    /// <inheritdoc cref="Contracts.IRepository{T}.CommitChangesAsync(CancellationToken)"/>
    public override Task<int> CommitChangesAsync(CancellationToken cancellationToken)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    public override ValueTask<T?> FindAsync(CancellationToken cancellationToken, params object[] keys)
    {
        return dbSet.FindAsync(keys, cancellationToken);
    }

    /// <inheritdoc cref="IEntityFrameworkRepository{TDbContext, T}.GetPagedResult(Expression{Func{T, bool}}, IPagedQuery{int},CancellationToken)" />
    public async Task<IPagedResult<int, T>> GetPagedResult(Expression<Func<T, bool>> expression, IPagedQuery<int> query, CancellationToken cancellationToken)
    {
        var q = this.Where(expression);
        var total = await q.CountAsync(cancellationToken);
        var maximumPages = 0;
        return Result.GetPaged(await q.Page(query.PageIndex.GetValueOrDefault(),
            query.TotalItemsPerPage.GetValueOrDefault()).ToArrayAsync(cancellationToken),
                query.PageIndex.GetValueOrDefault(), maximumPages, total);
    }

    /// <inheritdoc cref="IEntityFrameworkRepository{TDbContext, T}.GetPagedResult(Action{ExpressionStarter{T}}, IPagedQuery{int},CancellationToken)"/>
    public async Task<IPagedResult<int, T>> GetPagedResult(Action<ExpressionStarter<T>> queryBuilder, IPagedQuery<int> query, CancellationToken cancellationToken)
    {
        queryBuilder(QueryBuilder);
        
        var q = this.Where(QueryBuilder);
        var total = await q.CountAsync(cancellationToken);
        var maximumPages = 0;
        return Result.GetPaged(await q.Page(query.PageIndex.GetValueOrDefault(),
            query.TotalItemsPerPage.GetValueOrDefault()).ToArrayAsync(cancellationToken), 
                query.PageIndex.GetValueOrDefault(), maximumPages, total);
    }

    /// <inheritdoc cref="Contracts.IRepository{T}.NoTracking"/>
    public override bool NoTracking { set => ConfigureTracking(value); }

    /// <inheritdoc cref="EntityFrameworkRepositoryBase{TDbContext, T}.Context"/>
    public TDbContext Context { get; }
}
