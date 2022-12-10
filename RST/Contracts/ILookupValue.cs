namespace RST.Contracts;

/// <summary>
/// Represents a lookup value
/// </summary>
public interface ILookupValue : IIdentity, ICreated<DateTimeOffset>, IModified<DateTimeOffset>
{
    /// <summary>
    /// Gets the lookup name
    /// </summary>
    string? Name { get; set; }
    /// <summary>
    /// Gets the lookup display name
    /// </summary>
    string? DisplayName { get; set; }
    /// <summary>
    /// Gets a value representing whether the lookup value is marked as the default
    /// </summary>
    bool Default { get; set; }
}
