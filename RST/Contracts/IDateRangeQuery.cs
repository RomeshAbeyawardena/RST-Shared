namespace RST.Contracts;

/// <summary>
/// Represent a query comparing against a date range
/// </summary>
public interface IDateRangeQuery<T>
    where T: struct
{
    /// <summary>
    /// 
    /// </summary>
    T? StartDate { get; set; }
    /// <summary>
    /// 
    /// </summary>
    T? EndDate { get; set; }
}


/// <summary>
/// Represent a query comparing against a date range
/// </summary>
public interface IDateRangeQuery : IDateRangeQuery<DateTimeOffset>
{

}