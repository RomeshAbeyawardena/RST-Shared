using LinqKit;
using RST.Contracts;
using System.Collections;
using System.Linq.Expressions;

namespace RST;

public abstract class RepositoryBase<T> : IRepository<T>
{
    private IQueryable<T>? queryable;
    private readonly ExpressionStarter<T> queryBuilder;
    protected IQueryable<T> Queryable { set => queryable = value; }

    public RepositoryBase(IQueryable<T>? queryable  = null)
    {
        this.queryBuilder = PredicateBuilder.New<T>();
        queryBuilder.DefaultExpression = a => true;
        this.queryable = queryable;
    }

    public abstract void Add(T entity);
    public abstract void Update(T entity);

    public abstract bool NoTracking { set; }
    public Type ElementType => queryable?.ElementType ?? throw new NullReferenceException();
    public Expression Expression => queryable?.Expression ?? throw new NullReferenceException();
    public IQueryProvider Provider => queryable?.Provider ?? throw new NullReferenceException();
    public ExpressionStarter<T> QueryBuilder => queryBuilder; 
    public abstract Task<int> CommitChangesAsync(CancellationToken cancellationToken);

    public IEnumerator<T> GetEnumerator()
    {
        return (queryable ?? throw new NullReferenceException()).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return (queryable ?? throw new NullReferenceException()).GetEnumerator();
    }
}
