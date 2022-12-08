using RST.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

namespace RST.EntityFrameworkCore;

public abstract class EntityFrameworkRepositoryBase<TDbContext, T> : RepositoryBase<T>, IEntityFrameworkRepository<TDbContext, T>
    where TDbContext : DbContext
    where T : class
{
    private readonly DbSet<T> dbSet;

    public EntityFrameworkRepositoryBase(TDbContext context)
        : base()
    {
        dbSet = context.Set<T>();
        Queryable = dbSet;
        Context = context;
    }

    public override Task<int> CommitChangesAsync(CancellationToken cancellationToken)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }

    public TDbContext Context { get; }
}
