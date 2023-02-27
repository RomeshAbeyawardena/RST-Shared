using MediatR;

namespace RST.Contracts
{
    /// <summary>
    /// Represents a paged request
    /// </summary>
    /// <typeparam name="TModel">Returned page model of <typeparamref name="TModel"/></typeparam>
    public interface IPagedRequest<TModel> : IRequest<IPagedResult<TModel>>, IPagedQuery
    {
    }
}
