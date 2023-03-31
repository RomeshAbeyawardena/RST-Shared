using RST.Contracts;
using System.Globalization;
using System.Reflection;

namespace RST.Defaults;

/// <summary>
/// <inheritdoc cref="DefaultPropertyInfo"/>
/// </summary>
internal class DefaultPropertyInfo : PropertyInfo, IPropertyInfo
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

    public override PropertyAttributes Attributes => Property.Attributes;

    public override bool CanRead => Property.CanRead;

    public override bool CanWrite => Property.CanWrite;

    public override Type PropertyType => Property.PropertyType;

    public override Type? DeclaringType => Property.DeclaringType;

    public override string Name => Property.Name;

    public override Type? ReflectedType => Property.ReflectedType;

    public override MethodInfo[] GetAccessors(bool nonPublic)
    {
        return Property.GetAccessors(nonPublic);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
        return Property.GetCustomAttributes(inherit);
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        return Property.GetCustomAttributes(attributeType, inherit);
    }

    public override MethodInfo? GetGetMethod(bool nonPublic)
    {
        return Property.GetGetMethod(nonPublic);
    }

    public override ParameterInfo[] GetIndexParameters()
    {
        return Property.GetIndexParameters();
    }

    public override MethodInfo? GetSetMethod(bool nonPublic)
    {
        return Property.GetSetMethod(nonPublic);
    }

    public override object? GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
    {
        return Property.GetValue(obj, invokeAttr, binder, index, culture);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        return Property.IsDefined(attributeType, inherit);
    }

    public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
    {
        Property.SetValue(obj, value, invokeAttr, binder, index, culture);
    }
}
