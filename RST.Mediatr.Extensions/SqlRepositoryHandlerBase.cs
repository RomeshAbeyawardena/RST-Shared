using MediatR;
using Microsoft.EntityFrameworkCore;
using RST.Contracts;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace RST.Mediatr.Extensions;
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

    protected async Task<IEnumerable<TModel>> ProcessPagedQuery(Expression<Func<TModel, bool>> query, IPagedQuery request, CancellationToken cancellationToken)
    {
        var primaryQuery = Repository.Where(query);

        if (request.PageIndex.HasValue && request.TotalItemsPerPage.HasValue)
        {
            primaryQuery = primaryQuery.Page(request.PageIndex.Value, request.TotalItemsPerPage.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.OrderByField))
        {
            primaryQuery = primaryQuery.OrderBy($"{request.OrderByField} {GetOrderByValue(request.SortOrder)}");
        }

        return await primaryQuery.ToArrayAsync(cancellationToken);
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

    public SqlRepositoryHandlerBase(IRepository<TModel> repository)
    {
        Repository = repository;
    }

    public IRepository<TModel> Repository { get; }
}
