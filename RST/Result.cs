using RST.Contracts;
using RST.Defaults;

namespace RST;

/// <summary>
/// Represents an <see cref="IResult"/> helper
/// </summary>
public static class Result
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <param name="statusCode"></param>
    /// <param name="statusMessage"></param>
    /// <returns></returns>
    public static IResult<T> Create<T>(
        T? result = default, 
        int? statusCode = null, 
        string? statusMessage = null)
    {
        return new DefaultResult<T>()
        {
            IsSuccessful = result != null,
            Value = result,
            StatusCode = statusCode,
            StatusMessage = statusMessage
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="statusCode"></param>
    /// <param name="statusMessage"></param>
    /// <returns></returns>
    public static IResult<T> Create<T>(int statusCode, string statusMessage)
    {
        return Create<T>(statusCode, statusMessage);
    }
}
