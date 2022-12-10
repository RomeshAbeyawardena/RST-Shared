using Microsoft.Extensions.Internal;
using RST.Contracts;

namespace RST.Default;

public class DefaultClockProvider : IClockProvider
{
    public DefaultClockProvider(ISystemClock systemClock)
    {
        SystemClock = systemClock;
    }

    public ISystemClock SystemClock { get; }
    public DateTimeOffset UtcNow => SystemClock.UtcNow;
}
