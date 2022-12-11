using MediatR;
using Microsoft.EntityFrameworkCore;
using RST.Contracts;
using RST.Extensions;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

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
    /// <param name="repository"></param>
    protected SqlRepositoryHandlerBase(IRepository<TResponse> repository) : base(repository)
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
                Repository.Add(entity);
            }
            else
            {
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
    /// Processes a paged request and applies ordering
    /// </summary>
    /// <param name="query"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected Task<IEnumerable<TModel>> ProcessPagedQuery(Expression<Func<TModel, bool>> query, IPagedQuery request, CancellationToken cancellationToken)
    {
        return ProcessPagedQuery(query, request, null, cancellationToken);
    }

    /// <summary>
    /// Processes a paged request and applies ordering
    /// </summary>
    /// <param name="query"></param>
    /// <param name="request"></param>
    /// <param name="configureQuery"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task<IEnumerable<TModel>> ProcessPagedQuery(Expression<Func<TModel, bool>> query, IPagedQuery request, Func<IQueryable<TModel>, IQueryable<TModel>>? configureQuery, CancellationToken cancellationToken)
    {
        if (request.NoTracking.HasValue)
        {
            Repository.NoTracking = request.NoTracking.Value;
        }

        var primaryQuery = Repository.Where(query);

        if (request.PageIndex.HasValue && request.TotalItemsPerPage.HasValue)
        {
            primaryQuery = primaryQuery.Page(request.PageIndex.Value, request.TotalItemsPerPage.Value);
        }

        if (request.OrderByFields != null && request.OrderByFields.Any())
        {
            var order = GetOrderByValue(request.SortOrder);
            var orderList = string.Join(",", request.OrderByFields);
            primaryQuery = primaryQuery.OrderBy($"{orderList} {order}");
        }

        if (configureQuery != null)
        {
            primaryQuery = configureQuery.Invoke(primaryQuery);
        }

        return await primaryQuery.ToArrayAsync(cancellationToken);
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
    /// <param name="repository"></param>
    public SqlRepositoryHandlerBase(IRepository<TModel> repository)
    {
        Repository = repository;
    }

    /// <summary>
    /// 
    /// </summary>
    public IRepository<TModel> Repository { get; }
}
