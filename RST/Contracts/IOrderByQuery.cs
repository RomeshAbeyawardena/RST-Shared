using RST.Enumerations;

namespace RST.Contracts;

/// <summary>
/// Represents an order by query
/// </summary>
public interface IOrderByQuery : IDbQuery
{
    /// <summary>
    /// Gets or sets the order fields
    /// </summary>
    IEnumerable<string>? OrderByFields { get; set; }

    /// <summary>
    /// Gets or sets the sort order
    /// </summary>
    SortOrder? SortOrder { get; set; }
}
