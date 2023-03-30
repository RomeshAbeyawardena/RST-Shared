using RST.Attributes;
using RST.Contracts;
using RST.Extensions;
using System.Reflection;

namespace RST.Mediatr.Extensions.Extensions;

/// <summary>
/// 
/// </summary>
public static class PropertyInfoExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cache"></param>
    /// <param name="modelHasherFactory"></param>
    /// <returns></returns>
    public static IReadOnlyDictionary<PropertyInfo, IModelHasher?> GetModelHashers(this
        object model, IPropertyTypeProviderCache cache, IModelHasherFactory modelHasherFactory)
    {
        var dictionary = new Dictionary<PropertyInfo, IModelHasher?>();
        var propertyAttributes = model.GetType()
            .GetUnderliningAttributes<HashColumnAttribute>(cache);

        foreach (var (prop, attribute) in propertyAttributes)
        {
            var implementation = attribute != null && !string.IsNullOrWhiteSpace(attribute.HasherImplementation)
                ? modelHasherFactory.GetModelHasher(attribute.HasherImplementation)
                    ?? modelHasherFactory.GetDefault()
                : modelHasherFactory.GetDefault();
            dictionary.TryAdd(prop, implementation);
        }

        return dictionary;
    }
}
