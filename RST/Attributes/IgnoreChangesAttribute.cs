namespace RST.Attributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class IgnoreChangesAttribute : Attribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ignoreChanges"></param>
    public IgnoreChangesAttribute(bool ignoreChanges = true)
    {
        IgnoreChanges = ignoreChanges;
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IgnoreChanges { get; }
}
