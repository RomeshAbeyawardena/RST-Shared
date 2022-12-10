using RST.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace RST.EntityFrameworkCore;

public abstract class EntityFrameworkRepositoryBase<TDbContext, T> : RepositoryBase<T>, IEntityFrameworkRepository<TDbContext, T>
    where TDbContext : DbContext
    where T : class
{
    private readonly DbSet<T> dbSet;

    private void ConfigureTracking(bool noTracking)
    {
        Queryable = noTracking ? dbSet.AsNoTracking() : dbSet;
    }

    public EntityFrameworkRepositoryBase(TDbContext context)
        : base()
    {
        dbSet = context.Set<T>();
        Queryable = dbSet.AsNoTracking();
        Context = context;
    }

    public override void Add(T entity)
    {
        dbSet.Add(entity);
    }

    public override void Update(T entity)
    {
        dbSet.Update(entity);
    }

    public override Task<int> CommitChangesAsync(CancellationToken cancellationToken)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }

    public override bool NoTracking { set => ConfigureTracking(value); }

    public TDbContext Context { get; }
}
