namespace RST.Contracts;

/// <summary>
/// Represents a paged query
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IPagedQuery<T> : IOrderByQuery
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
}

/// <inheritdoc cref="IPagedQuery{T}"/>
public interface IPagedQuery : IPagedQuery<int>
{

}