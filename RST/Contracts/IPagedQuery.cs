using RST.Enumerations;

namespace RST.Contracts;

/// <summary>
/// Represents a paged query
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPagedQuery<T> : IDbQuery
    where T : struct
{
    /// <summary>
    /// Gets or sets the page index
    /// </summary>
    T? PageIndex { get; set; }
    /// <summary>
    /// Gets or sets the total items to display per page
    /// </summary>
    T? TotalItemsPerPage { get; set; }
    /// <summary>
    /// Gets or sets the order fields
    /// </summary>
    IEnumerable<string>? OrderByFields { get; set; }

    /// <summary>
    /// Gets or sets the sort order
    /// </summary>
    SortOrder? SortOrder { get; set; }
}

/// <inheritdoc cref="IPagedQuery{T}"/>
public interface IPagedQuery : IPagedQuery<int>
{

}