using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RST.Contracts;
using RST.DependencyInjection.Extensions;
using RST.DependencyInjection.Extensions.Attributes;
using RST.Extensions;
using RST.Mediatr.Extensions.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace RST.Mediatr.Extensions;

/// <summary>
/// Represents a repository handler
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public abstract class RepositoryHandlerBase<TRequest, TResponse> : RepositoryHandlerBase<TRequest, TResponse, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    protected RepositoryHandlerBase(IServiceProvider serviceProvider) 
        : base(serviceProvider)
    {
    }
}

/// <summary>
/// Represents a SQL Repository Handler
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <typeparam name="TModel"></typeparam>
public abstract class RepositoryHandlerBase<TRequest, TResponse, TModel> : EnableInjectionBase<InjectAttribute>, IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TModel : class
{
    /// <summary>
    /// 
    /// </summary>
    [Inject] protected IClockProvider? ClockProvider { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Inject] protected IRepository<TModel>? Repository { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Inject] protected IModelHasherFactory? ModelHasherFactory { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Inject] protected IPropertyTypeProviderCache? PropertyTypeProviderCache { get; set; }

    /// <summary>
    /// Configures no tracking against the repository based upon the <paramref name="request"/> parameter
    /// </summary>
    /// <param name="request">Request used to configure the repository</param>
    protected void ConfigureNoTracking(IDbQuery request)
    {
        if (request.NoTracking.HasValue)
        {
            Repository!.NoTracking = request.NoTracking.Value;
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
    /// Processes a save command and applies the appropriate add/update behaviour against the underlining provider
    /// </summary>
    /// <typeparam name="TDbCommand"></typeparam>
    /// <param name="command"></param>
    /// <param name="convertToModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    protected internal async Task<TModel> ProcessSave<TDbCommand>(TDbCommand command, Func<TDbCommand, TModel?> convertToModel, CancellationToken cancellationToken)
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
                    created.Created = ClockProvider!.UtcNow;
                }

                Repository!.Add(entity);
            }
            else
            {
                if(entity is IModified modified)
                {
                    modified.Modified = ClockProvider!.UtcNow;
                }

                var foundEntity = await Repository!.FindAsync(cancellationToken, identity.Id);

                if (foundEntity != null)
                {

                    if(!this.ValidateHash(PropertyTypeProviderCache!, ModelHasherFactory!,
                        foundEntity, entity))
                    {
                        throw new ValidationFailureException(dict => dict.Add("Hash", "Hashes don't match"));
                    }

                    if (foundEntity.HasChanges(entity, out var changes))
                    {
                        foundEntity.CommitChanges(changes);
                    }
                }
            }
        }
        else
            Repository!.Add(entity);

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

        return await OrderByQuery(this.Repository!.Where(query), orderByQuery).ToArrayAsync(cancellationToken);
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

        return Repository!.GetPagedResult(query, request, cancellationToken);
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
    /// <param name="serviceProvider"></param>
    public RepositoryHandlerBase(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        ConfigureInjection();
    }
}
