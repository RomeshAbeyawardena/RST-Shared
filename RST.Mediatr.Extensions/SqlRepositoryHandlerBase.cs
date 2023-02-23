using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RST.Contracts;
using RST.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace RST.Mediatr.Extensions;

/// <summary>
/// Represents a SQL Repository Handler
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class SqlRepositoryHandlerBase<TRequest, TResponse> : SqlRepositoryHandlerBase<TRequest, TResponse, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="clockProvider"></param>
    /// <param name="repository"></param>
    protected SqlRepositoryHandlerBase(IClockProvider clockProvider, IRepository<TResponse> repository) : base(clockProvider, repository)
    {
    }
}

/// <summary>
/// Represents a SQL Repository Handler
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <typeparam name="TModel"></typeparam>
public abstract class SqlRepositoryHandlerBase<TRequest, TResponse, TModel> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TModel : class
{
    private readonly IClockProvider clockProvider;

    /// <summary>
    /// Configures no tracking against the repository based upon the <paramref name="request"/> parameter
    /// </summary>
    /// <param name="request">Request used to configure the repository</param>
    protected void ConfigureNoTracking(IDbQuery request)
    {
        if (request.NoTracking.HasValue)
        {
            Repository.NoTracking = request.NoTracking.Value;
        }
    }

    /// <summary>
    /// Executes an order by against the <paramref name="query"/>
    /// </summary>
    /// <param name="query">Query to extend</param>
    /// <param name="request">Request used to extend query</param>
    /// <returns></returns>
    protected IQueryable<TModel> OrderByQuery(IQueryable<TModel> query, IOrderByQuery request)
    {
        if (request.OrderByFields != null && request.OrderByFields.Any())
        {
            var order = GetOrderByValue(request.SortOrder);
            var orderList = string.Join(",", request.OrderByFields);
            return query.OrderBy($"{orderList} {order}");
        }

        return query;
    }

    /// <summary>
    /// Filters a date range object
    /// </summary>
    /// <param name="query">Query to extend</param>
    /// <param name="request">Request used to extend</param>
    /// <returns></returns>
    protected Expression<Func<TModel, bool>> FilterByDateRange(Expression<Func<TModel, bool>> query, IDateRangeQuery request)
    {
        if (request.StartDate.HasValue || request.EndDate.HasValue)
        {
            query = query.And(DefineDateRangeQuery(request));
        }
        return query;
    }

    /// <summary>
    /// Define the date range query to filter <typeparamref name="TModel"/> by a date range 
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    protected virtual Expression<Func<TModel, bool>> DefineDateRangeQuery(IDateRangeQuery query)
    {
        return a => true;
    }

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
    
    /// <summary>
    /// Processes a save command and applies the approriate add/update behaviour against the underlining provider
    /// </summary>
    /// <typeparam name="TDbCommand"></typeparam>
    /// <param name="command"></param>
    /// <param name="convertToModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    protected async Task<TModel> ProcessSave<TDbCommand>(TDbCommand command, Func<TDbCommand, TModel?> convertToModel, CancellationToken cancellationToken)
        where TDbCommand : IDbCommand
    {
        var entity = convertToModel(command);
        
        if(entity == null)
        {
            throw new NullReferenceException();
        }

        if(entity is IIdentity identity)
        {
            if(identity.Id == Guid.Empty)
            {
                if(entity is ICreated created)
                {
                    created.Created = clockProvider.UtcNow;
                }

                Repository.Add(entity);
            }
            else
            {
                if(entity is IModified modified)
                {
                    modified.Modified = clockProvider.UtcNow;
                }

                var foundEntity = await Repository.FindAsync(cancellationToken, identity.Id);

                if (foundEntity != null)
                {
                    foundEntity.HasChanges(entity, out var changes);
                    foundEntity.CommitChanges(changes);
                }
            }
        }
        else
            Repository.Add(entity);

        if (command.CommitChanges)
        {
            await Repository.CommitChangesAsync(cancellationToken);
        }

        return entity;
    }

    /// <summary>
    /// Process orderable queryable
    /// </summary>
    /// <param name="query">Query to filter and order</param>
    /// <param name="orderByQuery">Request object to apply ordering and filtering</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task<IEnumerable<TModel>> ProcessOrderableQuery(Expression<Func<TModel, bool>> query, IOrderByQuery orderByQuery, CancellationToken cancellationToken)
    {
        ConfigureNoTracking(orderByQuery);

        if (orderByQuery is IDateRangeQuery dateRangeQuery)
        {
            query = FilterByDateRange(query, dateRangeQuery);
        }

        return await OrderByQuery(this.Repository.Where(query), orderByQuery).ToArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Processes a paged request and applies ordering
    /// </summary>
    /// <param name="query">The query to filter by</param>
    /// <param name="request">The request object</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected Task<IPagedResult<TModel>> ProcessPagedQuery(Expression<Func<TModel, bool>> query, IPagedQuery request, CancellationToken cancellationToken)
    {
        ConfigureNoTracking(request);

        if (request is IDateRangeQuery dateRangeQuery)
        {
            query = FilterByDateRange(query, dateRangeQuery);
        }

        return Repository.GetPagedResult(query, request, cancellationToken);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clockProvider"></param>
    /// <param name="repository"></param>
    public SqlRepositoryHandlerBase(IClockProvider clockProvider, IRepository<TModel> repository)
    {
        this.clockProvider = clockProvider;
        Repository = repository;
    }

    /// <summary>
    /// 
    /// </summary>
    public IRepository<TModel> Repository { get; }
}
