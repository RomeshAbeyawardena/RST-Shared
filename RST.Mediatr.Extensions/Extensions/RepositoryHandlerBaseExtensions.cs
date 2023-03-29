using MediatR;
using MessagePack;
using RST.Attributes;
using RST.Contracts;
using RST.Extensions;
using System.Reflection;

namespace RST.Mediatr.Extensions;

/// <summary>
/// 
/// </summary>
public static class RepositoryHandlerBaseExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="repositoryHandler"></param>
    /// <param name="cache"></param>
    /// <param name="modelHasherFactory"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    public static bool ValidateHash<TRequest, TResponse, TModel>(
        this RepositoryHandlerBase<TRequest, TResponse, TModel> repositoryHandler,
        IPropertyTypeProviderCache cache, IModelHasherFactory modelHasherFactory, TModel model)
        where TRequest : IRequest<TResponse>
        where TModel : class
    {
        var modelType = typeof(TModel);
        IEnumerable<PropertyInfo>? properties; 
        if(!cache.TryGetValue(modelType, out properties))
        {
            cache.AddOrUpdate(modelType, properties = modelType.GetAllProperties());
        }

        var propertyAttributes = new Dictionary<PropertyInfo, Attribute?>();
        foreach( var property in properties)
        {
            if(!property.HasAttribute<HashColumnAttribute>(out var attribute))
            {
                propertyAttributes.TryAdd(property, attribute);
            }
        }
        
        //modelHasherFactory.GetModelHasher()

        return false;
    }
}
