using RST.Contracts;

namespace RST.Defaults;

/// <summary>
/// Represents a default paged result
/// </summary>
/// <typeparam name="TResult"></typeparam>
public record DefaultPagedResult<TResult> : DefaultPagedResult<int, TResult>, IPagedResult<TResult>
{

}

/// <summary>
/// Represents a default paged result
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
public record DefaultPagedResult<T, TResult> : DefaultResult<IEnumerable<TResult>>, IPagedResult<T, TResult>
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
