using RST.Contracts;

namespace RST.AspNetCore.Extensions.Contracts;

/// <summary>
/// Represents an application identity
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TTimeStamp"></typeparam>
public interface IApplicationIdentity<TKey, TTimeStamp> : IIdentity<TKey>, ICreated<TTimeStamp>, IModified<TTimeStamp>
    where TKey : struct
    where TTimeStamp : struct
{
    /// <summary>
    /// Gets or sets the application name
    /// </summary>
    string Name { get; set; }
    /// <summary>
    /// Gets or sets the public key
    /// </summary>
    string PublicKey { get; set; }
    /// <summary>
    /// Gets or sets the access token
    /// </summary>
    string AccessToken { get; set; }
    /// <summary>
    /// Gets or sets the Is enabled flag
    /// </summary>
    bool IsEnabled { get; set; }
}
