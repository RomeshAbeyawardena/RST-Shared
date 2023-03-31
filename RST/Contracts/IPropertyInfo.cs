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
    /// The underlining <see cref="PropertyInfo"/>
    /// </summary>
    PropertyInfo Property { get; }
    /// <summary>
    /// The related descendant of the <see cref="System.Type"/>
    /// </summary>
    Type? RelatedType { get; }
}
