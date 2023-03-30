using AutoMapper;
using RST.Contracts;
using RST.Defaults;

namespace RST.Automapper.Extensions;
/// <summary>
/// 
/// </summary>
public static class MappingExpression
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="profile"></param>
    /// <returns></returns>
    public static IMappingExpression<IPagedResult<TSource>, IPagedResult<TDestination>> CreatePagedMapping<TSource, TDestination>(
        this Profile profile)
    {
        IEnumerable<TDestination>? destination;
        return profile.CreateMap<IPagedResult<TSource>, IPagedResult<TDestination>>()
            .ConstructUsing((c, r) => new DefaultPagedResult<TDestination>
            {
                IsSuccessful = c.IsSuccessful,
                TotalItems = c.TotalItems,
                Message = c.Message,
                NumberOfPages = c.NumberOfPages,
                PageNumber = c.PageNumber,
                Results = (destination = r.Mapper.Map<IEnumerable<TDestination>>(c.Results)),
                StatusCode = c.StatusCode,
                Value = destination
            });
    }
}
