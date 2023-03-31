using RST.Contracts;
using System.Reflection;

namespace RST.Extensions;

/// <summary>
/// Extension methods for <see cref="PropertyInfo"/>
/// </summary>
public static class PropertyInfoExtensions
{
    /// <summary>
    /// Determines whether the attribute exists and outputs the attribute in <paramref name="attribute"/> parameter
    /// </summary>
    /// <param name="property">The instance of <see cref="PropertyInfo"/> to check</param>
    /// <param name="attributeType">The <see cref="Attribute"/> type to find and output</param>
    /// <param name="attribute">The outputted attribute</param>
    /// <returns></returns>
    public static bool HasAttribute(
        this PropertyInfo property,
        Type attributeType, out object? attribute)
    {
        attribute = property.GetCustomAttribute(attributeType);
        return attribute != null;
    }

    /// <summary>
    /// Determines whether the attribute exists and outputs the attribute in <paramref name="attribute"/> parameter
    /// </summary>
    /// <typeparam name="TAttribute">The instance of <see cref="PropertyInfo"/> as <typeparamref name="TAttribute"/> to check</typeparam>
    /// <param name="property">The <see cref="Attribute"/> as <typeparamref name="TAttribute"/> type to find and output</param>
    /// <param name="attribute">The outputted attribute</param>
    /// <returns></returns>
    public static bool HasAttribute<TAttribute>(
        this PropertyInfo property,
        out TAttribute? attribute)
        where TAttribute : Attribute
    {
        attribute = property.GetCustomAttribute<TAttribute>();
        return attribute != null;
    }

    /// <summary>
    /// Gets 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="cache"></param>
    /// <param name="filterExpression"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetAllProperties(this Type type, IPropertyTypeProviderCache? cache = null, Func<PropertyInfo, bool>? filterExpression = null)
    {
        IEnumerable<PropertyInfo> properties;

        if (cache == null || !cache.TryGetValue(type, out var props))
        {
            properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            cache?.AddOrUpdate(type, properties);
        }
        else
        {
            properties = props;
        }

        if (filterExpression == null)
        {
            return properties;
        }

        return properties.Where(filterExpression ?? (a => true));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="type"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public static IReadOnlyDictionary<PropertyInfo, TAttribute> GetUnderliningAttributes<TAttribute>(
        this Type type,
        IPropertyTypeProviderCache? cache = null)
        where TAttribute : Attribute
    {
        var dictionary = new Dictionary<PropertyInfo, TAttribute>();
        IEnumerable<PropertyInfo>? properties;

        properties = type.GetAllProperties(cache);
        
        var allProperties = properties.Union(type.GetInterfaces().SelectMany(t => t.GetAllProperties(cache)));

        foreach (var property in allProperties)
        {
            if (property.HasAttribute<TAttribute>(out var attribute) && attribute != null)
            {
                dictionary.Add(property, attribute);
            }
        }

        return dictionary;
    }
}
