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
    /// <returns></returns>
    public static bool HasChanges(this object target, object source, out IEnumerable<IObjectChange> changes)
    {
        var changeList = new List<IObjectChange>();
        changes = changeList;

        var targetType = target.GetType();
        var sourceType = source.GetType();

        var targetProperties = targetType.GetProperties();
        var sourceProperties = targetProperties;
        if(targetType != sourceType)
        {
            sourceProperties = sourceType.GetProperties();
        }

        foreach(var property in sourceProperties)
        {
            if(!property.CanRead && property.CanWrite)
            {
                continue;
            }

            var targetProperty = sourceProperties.FirstOrDefault(p => p.Name == property.Name && p.PropertyType == property.PropertyType);

            if(targetProperty == null)
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
                changeList.Add(new DefaultObjectChange { 
                    HasChanged = true,
                    NewValue = sourceValue,
                    OldValue = targetValue,
                    SourceProperty = property,
                    TargetProperty = targetProperty
                });
            }
        }

        return changeList.Count > 0;
    }

    /// <summary>
    /// Commits <paramref name="changes"/> to value to <paramref name="target"/> parameter
    /// </summary>
    /// <param name="target">The target to apply changes to</param>
    /// <param name="changes">A list of changes detected</param>
    public static void CommitChanges(this object target, IEnumerable<IObjectChange> changes)
    {
        foreach(var change in changes)
        {
            if (!change.HasChanged)
            {
                continue;
            }

            change.TargetProperty.SetValue(target, change.NewValue);
        }
    }
}
