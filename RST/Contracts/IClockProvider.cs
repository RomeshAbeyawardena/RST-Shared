using Microsoft.Extensions.Internal;

namespace RST.Contracts;

/// <summary>
/// Represents a clock provider
/// </summary>
public interface IClockProvider
{
    /// <summary>
    /// Gets an instance of <see cref="ISystemClock"/> that is used to retrieve the current <see cref="DateTimeOffset"/>
    /// </summary>
    ISystemClock SystemClock { get; }
    /// <summary>
    /// Gets the UTC <see cref="DateTimeOffset"/>
    /// </summary>
    DateTimeOffset UtcNow { get; }
}
