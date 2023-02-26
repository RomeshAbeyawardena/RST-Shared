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

    private static string GetOrderByValue(Enumerations.SortOrder? sortOrder)
    {
        if (!sortOrder.HasValue)
        {
            sortOrder = Enumerations.SortOrder.Ascending;
        }

        return sortOrder switch
        {
            Enumerations.SortOrder.Ascending => "asc",
            Enumerations.SortOrder.Descending => "desc",
            _ => string.Empty,
        };
    }

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

    /// <inheritdoc cref="IRepository{T}.GetPagedResult(Expression{Func{T, bool}}, IPagedQuery{int}, CancellationToken)" />
    public override async Task<IPagedResult<T>> GetPagedResult(Expression<Func<T, bool>> expression, IPagedQuery<int> query, CancellationToken cancellationToken)
    {
        var q = this.Where(expression);

        if (query.OrderByFields != null && query.OrderByFields.Any())
        {
            var order = GetOrderByValue(query.SortOrder);
            var orderList = string.Join(",", query.OrderByFields);
            q = q.OrderBy($"{orderList} {order}");
        }
        var total = await q.CountAsync(cancellationToken);


        if (!query.TotalItemsPerPage.HasValue || !query.PageIndex.HasValue)
        {
            return Result.GetPaged(await q.ToArrayAsync(cancellationToken), 1, 1, total);
        }
        else
        {
            
            var maximumPages = query.TotalItemsPerPage.HasValue
                ? Convert.ToInt32(Math.Ceiling((decimal)query.TotalItemsPerPage / total))
                : 1;

            return Result.GetPaged(await q.Page(query.PageIndex!.Value,
                query.TotalItemsPerPage!.Value).ToArrayAsync(cancellationToken),
                    query.PageIndex.Value, maximumPages, total);
        }
    }

    /// <inheritdoc cref="IRepository{T}.GetPagedResult(Action{ExpressionStarter{T}}, IPagedQuery{int}, CancellationToken)"/>
    public override Task<IPagedResult<T>> GetPagedResult(Action<ExpressionStarter<T>> queryBuilder, IPagedQuery<int> query, CancellationToken cancellationToken)
    {
        queryBuilder(QueryBuilder);
        return GetPagedResult(queryBuilder, query, cancellationToken);
    }

    /// <inheritdoc cref="Contracts.IRepository{T}.NoTracking"/>
    public override bool NoTracking { set => ConfigureTracking(value); }

    /// <inheritdoc cref="EntityFrameworkRepositoryBase{TDbContext, T}.Context"/>
    public TDbContext Context { get; }
}
