
namespace RST.Contracts;

/// <summary>
/// Represents a result
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets error or status message
    /// </summary>
    string? Message { get; }
    /// <summary>
    /// Gets the status code
    /// </summary>
    int? StatusCode { get; }
    /// <summary>
    /// Gets the result value
    /// </summary>
    object? Value { get; }
    /// <summary>
    /// Gets a value that determines whether the request was successful
    /// </summary>
    bool IsSuccessful { get; }
}

/// <summary>
/// Represents a result of <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResult<T> : IResult
{
    /// <summary>
    /// Gets the result value of <typeparamref name="T"/>
    /// </summary>
    new T? Value { get; }
}