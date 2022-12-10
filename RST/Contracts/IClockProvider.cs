using Microsoft.Extensions.Internal;

namespace RST.Contracts;

public interface IClockProvider
{
    ISystemClock SystemClock { get; }
    DateTimeOffset UtcNow { get; }
}
