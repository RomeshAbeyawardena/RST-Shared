using System.Reflection;

namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface IObjectChange
{
    /// <summary>
    /// Gets the source <see cref="PropertyInfo"/> for the change
    /// </summary>
    PropertyInfo SourceProperty { get; }
    /// <summary>
    /// Gets the target <see cref="PropertyInfo"/> for the change
    /// </summary>
    PropertyInfo TargetProperty { get; }
    /// <summary>
    /// Gets the new value
    /// </summary>
    object NewValue { get; }

    /// <summary>
    /// Gets the old value
    /// </summary>
    object OldValue { get; }
    /// <summary>
    /// Gets a value determining whether there are any 
    /// </summary>
    bool HasChanged { get; }
}
