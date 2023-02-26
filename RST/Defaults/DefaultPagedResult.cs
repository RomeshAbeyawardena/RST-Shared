using RST.Contracts;
using System.Collections;

namespace RST.Defaults;

/// <summary>
/// Represents a default paged result
/// </summary>
/// <typeparam name="TResult"></typeparam>
public class DefaultPagedResult<TResult> : DefaultPagedResult<int, TResult>, IPagedResult<TResult>
{

}

/// <summary>
/// Represents a default paged result
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class DefaultPagedResult<T, TResult> : DefaultResult<IEnumerable<T>>, IPagedResult<T, TResult>
    where T : struct
{
    /// <summary>
    /// Initializes an instance of <see cref="DefaultPagedResult{T, TResult}"/> 
    /// </summary>
    public DefaultPagedResult()
    {
        Results = new List<TResult>();
    }

    ///<inheritdoc />
    public T PageNumber { get; set; }

    ///<inheritdoc />
    public T NumberOfPages { get; set; }

    ///<inheritdoc />
    public T TotalItems { get; set; }

    ///<inheritdoc />
    public IEnumerable<TResult> Results { get; set; }
    
    IEnumerable<TResult>? IResult<IEnumerable<TResult>>.Value => Results;
}
