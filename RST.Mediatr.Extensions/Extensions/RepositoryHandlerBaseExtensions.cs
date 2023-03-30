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
    /// <param name="modifiedModel"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static bool ValidateHash<TRequest, TResponse, TModel>(
        this RepositoryHandlerBase<TRequest, TResponse, TModel> repositoryHandler,
        IPropertyTypeProviderCache cache, IModelHasherFactory modelHasherFactory, 
        TModel model, TModel modifiedModel, object? options = null)
        where TRequest : IRequest<TResponse>
        where TModel : class
    {
        var modelType = typeof(TModel);

        var propertyAttributes = modelType.GetUnderliningAttributes<HashColumnAttribute>(cache);

        var truthTable = new List<bool>();
        foreach(var (prop, attribute) in propertyAttributes)
        {
            var implementation = attribute != null && !string.IsNullOrWhiteSpace(attribute.HasherImplementation)
                ? modelHasherFactory.GetModelHasher(attribute.HasherImplementation) 
                    ?? modelHasherFactory.GetDefault()
                : modelHasherFactory.GetDefault();
            var value = prop.GetValue(modifiedModel)?.ToString();
            truthTable.Add(
                implementation.CompareHash(model, options, value));
        }
        
        return !truthTable.Any() || truthTable.All(a => a);
    }
}
