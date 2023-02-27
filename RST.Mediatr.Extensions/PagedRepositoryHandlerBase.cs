using MediatR;
using RST.Contracts;

namespace RST.Mediatr.Extensions;

/// <summary>
/// Represents an abstract paged repository request handler
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TModel"></typeparam>
public abstract class PagedRepositoryHandlerBase<TRequest, TModel>
    : SqlRepositoryHandlerBase<TRequest, IPagedResult<TModel>, TModel>
    where TRequest: IRequest<IPagedResult<TModel>>
    where TModel: class
{
    /// <summary>
    /// Initialises an inherited instance of <see cref="PagedRepositoryHandlerBase{TRequest, TModel}"/>
    /// </summary>
    /// <param name="clockProvider">Instance of clock provider</param>
    /// <param name="repository">Instance of SQL repository</param>
    public PagedRepositoryHandlerBase(IClockProvider clockProvider, IRepository<TModel> repository) : base(clockProvider, repository)
    {
    }
}
