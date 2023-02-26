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
    /// <param name="filterExpression"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetInheritedProperties(this Type type, Func<PropertyInfo, bool>? filterExpression = null)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (filterExpression == null)
        {
            return properties;
        }

        return properties.Where(filterExpression ?? (a => true));
    }
}
