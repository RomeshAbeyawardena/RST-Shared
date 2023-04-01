using System.Globalization;
using System.Reflection;

namespace RST.Contracts;

/// <summary>
/// Represents a <see cref="System.Type"/> property
/// </summary>
public interface IPropertyInfo
{
    /// <summary>
    /// Referenced type this property belongs to
    /// </summary>
    Type Type { get; }

    /// <summary>
    /// 
    /// </summary>
    Type? DeclaringType { get; }

    /// <summary>
    /// 
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 
    /// </summary>
    Type? ReflectedType { get; }

    /// <summary>
    /// The underlining <see cref="PropertyInfo"/>
    /// </summary>
    PropertyInfo Property { get; }
    /// <summary>
    /// The related descendant of the <see cref="System.Type"/>
    /// </summary>
    Type? RelatedType { get; }
    /// <summary>
    /// 
    /// </summary>
    bool CanRead { get; }
    /// <summary>
    /// 
    /// </summary>
    bool CanWrite { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="invokeAttr"></param>
    /// <param name="binder"></param>
    /// <param name="index"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    object? GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    object? GetValue(object? obj);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attributeType"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    bool IsDefined(Type attributeType, bool inherit);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    /// <param name="invokeAttr"></param>
    /// <param name="binder"></param>
    /// <param name="index"></param>
    /// <param name="culture"></param>
    void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    void SetValue(object? obj, object? value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    bool Equals(PropertyInfo propertyInfo);
}
