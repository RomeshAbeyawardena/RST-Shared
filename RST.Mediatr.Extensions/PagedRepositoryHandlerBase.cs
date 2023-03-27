using MediatR;
using RST.Contracts;

namespace RST.Mediatr.Extensions;

/// <summary>
/// Represents an abstract paged repository request handler
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TModel"></typeparam>
public abstract class PagedRepositoryHandlerBase<TRequest, TModel>
    : RepositoryHandlerBase<TRequest, IPagedResult<TModel>, TModel>
    where TRequest: IRequest<IPagedResult<TModel>>
    where TModel: class
{
    /// <summary>
    /// Initialises an inherited instance of <see cref="PagedRepositoryHandlerBase{TRequest, TModel}"/>
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="clockProvider">Instance of clock provider</param>
    /// <param name="repository">Instance of SQL repository</param>
    public PagedRepositoryHandlerBase(IServiceProvider serviceProvider, IClockProvider clockProvider, IRepository<TModel> repository) : base(serviceProvider, clockProvider, repository)
    {
    }
}
