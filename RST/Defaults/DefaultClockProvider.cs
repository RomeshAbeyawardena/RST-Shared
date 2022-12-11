using Microsoft.Extensions.Internal;
using RST.Contracts;

namespace RST.Defaults;

/// <inheritdoc cref="IClockProvider"/>
public class DefaultClockProvider : IClockProvider
{
    /// <summary>
    /// Creates an instance of <see cref="DefaultClockProvider"/>
    /// </summary>
    /// <param name="systemClock"></param>
    public DefaultClockProvider(ISystemClock systemClock)
    {
        SystemClock = systemClock;
    }

    /// <inheritdoc cref="IClockProvider.SystemClock"/>
    public ISystemClock SystemClock { get; }
    /// <summary>    
    /// <inheritdoc cref="IClockProvider.UtcNow"/>
    /// </summary>
    public DateTimeOffset UtcNow => SystemClock.UtcNow;
}
