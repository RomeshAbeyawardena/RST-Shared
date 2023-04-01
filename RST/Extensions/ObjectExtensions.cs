using RST.Attributes;
using RST.Contracts;
using RST.Defaults;

namespace RST.Extensions;

/// <summary>
/// 
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Gets a list of <see cref="IObjectChange"/> <paramref name="changes"/> 
    /// </summary>
    /// <param name="target">The target to apply changes to</param>
    /// <param name="source">The source to compare changes from</param>
    /// <param name="changes">A list of changes detected</param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public static bool HasChanges(this object target, object source,
        out IEnumerable<IObjectChange> changes, IPropertyTypeProviderCache? cache = null)
    {
        var changeList = new List<IObjectChange>();
        changes = changeList;

        var targetType = target.GetType();
        var sourceType = source.GetType();
        
        var properties = sourceType.GetUnderliningAttributes<IgnoreChangesAttribute>(cache);

        var targetProperties = targetType.GetAllProperties(cache);
        var sourceProperties = targetProperties;
        if (targetType != sourceType)
        {
            sourceProperties = sourceType.GetAllProperties(cache);
        }

        foreach (var property in sourceProperties)
        {
            if(properties.Any(p => p.Key.Equals(property)))
            {
                continue;
            }

            if (!property.CanRead && property.CanWrite)
            {
                continue;
            }

            var targetProperty = sourceProperties.FirstOrDefault(p => p.Name == property.Name && p.Type == property.Type);

            if (targetProperty == null)
            {
                continue;
            }

            var sourceValue = property.GetValue(source);
            var targetValue = property.GetValue(target);

            var isSourceNotNullAndSourceNotEqualToTarget = sourceValue != null
                ? !sourceValue.Equals(targetValue)
                : targetValue != null && !targetValue.Equals(sourceValue);

            if (isSourceNotNullAndSourceNotEqualToTarget)
            {
                changeList.Add(new DefaultObjectChange
                {
                    HasChanged = true,
                    NewValue = sourceValue,
                    OldValue = targetValue,
                    SourceProperty = property.Property,
                    TargetProperty = targetProperty.Property
                });
            }
        }

        return changeList.Count > 0;
    }

    /// <summary>
    /// Commits <paramref name="changes"/> to the source value to <paramref name="target"/> parameter
    /// </summary>
    /// <param name="target">The target to apply changes to</param>
    /// <param name="changes">A list of changes detected</param>
    public static void CommitChanges(this object target, IEnumerable<IObjectChange> changes)
    {
        foreach (var change in changes)
        {
            if (!change.HasChanged)
            {
                continue;
            }

            change.TargetProperty.SetValue(target, change.NewValue);
        }
    }
}
