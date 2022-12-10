using MediatR;
using Microsoft.EntityFrameworkCore;
using RST.Contracts;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace RST.Mediatr.Extensions;

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

        switch (sortOrder)
        {
            case Enumerations.SortOrder.Ascending:
                return "asc";
            case Enumerations.SortOrder.Descending:
                return "desc";
            default:
                return string.Empty;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="query"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task<IEnumerable<TModel>> ProcessPagedQuery(Expression<Func<TModel, bool>> query, IPagedQuery request, CancellationToken cancellationToken)
    {
        if(request.NoTracking.HasValue)
        {
            Repository.NoTracking = request.NoTracking.Value;
        }

        var primaryQuery = Repository.Where(query);

        if (request.PageIndex.HasValue && request.TotalItemsPerPage.HasValue)
        {
            primaryQuery = primaryQuery.Page(request.PageIndex.Value, request.TotalItemsPerPage.Value);
        }

        if (request.OrderByFields != null)
        {
            var order = GetOrderByValue(request.SortOrder);
            var orderList = string.Join(",", request.OrderByFields);
            primaryQuery = primaryQuery.OrderBy($"{orderList} {order}");
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
