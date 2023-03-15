using RST.Contracts;

namespace RST.AspNetCore.Extensions.Contracts;

/// <summary>
/// 
/// </summary>
public interface IApplicationIdentity : System.Security.Principal.IIdentity
{
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

/// <summary>
/// Represents an application identity
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TTimeStamp"></typeparam>
public interface IApplicationIdentity<TKey, TTimeStamp> : IIdentity<TKey>, ICreated<TTimeStamp>, IModified<TTimeStamp>, IApplicationIdentity
    where TKey : struct
    where TTimeStamp : struct
{
    
}
