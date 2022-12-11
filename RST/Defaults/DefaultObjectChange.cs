using RST.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RST.Defaults;

/// <summary>
/// Represents an object change
/// </summary>
public class DefaultObjectChange : IObjectChange
{
    /// <inheritdoc cref="IObjectChange.SourceProperty"/>
    [AllowNull]
    public PropertyInfo SourceProperty { get; init; }

    /// <inheritdoc cref="IObjectChange.TargetProperty"/>
    [AllowNull]
    public PropertyInfo TargetProperty { get; init; }

    /// <inheritdoc cref="IObjectChange.NewValue"/>
    [AllowNull]
    public object NewValue { get; init; }

    /// <inheritdoc cref="IObjectChange.OldValue"/>
    [AllowNull]
    public object OldValue { get; init; }

    /// <inheritdoc cref="IObjectChange.HasChanged"/>
    [AllowNull]
    public bool HasChanged { get; init; }
}
