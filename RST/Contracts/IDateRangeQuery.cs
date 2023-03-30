namespace RST.Contracts;

/// <summary>
/// Represent a query comparing against a date range
/// </summary>
public interface IDateRangeQuery<T> : IDbQuery
    where T : struct
{
    /// <summary>
    /// Gets or sets the start date to filter
    /// </summary>
    T? StartDate { get; set; }
    /// <summary>
    /// Gets or sets the end date to filter
    /// </summary>
    T? EndDate { get; set; }
}


/// <summary>
/// Represent a query comparing against a date range
/// </summary>
public interface IDateRangeQuery : IDateRangeQuery<DateTimeOffset>
{

}