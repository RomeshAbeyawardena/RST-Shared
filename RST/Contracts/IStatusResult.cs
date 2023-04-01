namespace RST.Contracts;

/// <summary>
/// <inheritdoc cref="IStatusObjectResult"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IStatusResult<T> : IStatusObjectResult
{
    /// <summary>
    /// <inheritdoc cref="IStatusObjectResult.Result"/>
    /// </summary>
    new T Result { get; set; }
}
