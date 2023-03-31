using RST.Contracts;
using System.Reflection;

namespace RST.Defaults;

/// <summary>
/// <inheritdoc cref="DefaultPropertyInfo"/>
/// </summary>
internal record DefaultPropertyInfo : IPropertyInfo
{
    public DefaultPropertyInfo(Type type, PropertyInfo propertyInfo, Type? relatedProperty = null)
    {
        Type = type;
        Property = propertyInfo;
        RelatedType = relatedProperty;
    }
    /// <summary>
    /// <inheritdoc cref="IPropertyInfo.Type"/>
    /// </summary>
    public Type Type { get; }
    /// <summary>
    /// <inheritdoc cref="IPropertyInfo.Property"/>
    /// </summary>
    public PropertyInfo Property { get; }
    /// <summary>
    /// <inheritdoc cref="IPropertyInfo.RelatedType"/>
    /// </summary>
    public Type? RelatedType { get; }
}
