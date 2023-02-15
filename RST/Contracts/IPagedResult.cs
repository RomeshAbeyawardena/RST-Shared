namespace RST.Contracts;

/// <summary>
/// Represents a paged result
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IPagedResult<T, TResult> : IResult<IEnumerable<TResult>>
    where T : struct
{
    /// <summary>
    /// 
    /// </summary>
    T PageNumber { get; }
    /// <summary>
    /// 
    /// </summary>
    T NumberOfPages { get; }
    /// <summary>
    /// 
    /// </summary>
    T TotalItems { get; }
    /// <summary>
    /// Gets a list of results
    /// </summary>
    IEnumerable<TResult> Results { get; }

}

/// <summary>
/// Represents a paged result
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IPagedResult<TResult> : IPagedResult<int, TResult>
{

}