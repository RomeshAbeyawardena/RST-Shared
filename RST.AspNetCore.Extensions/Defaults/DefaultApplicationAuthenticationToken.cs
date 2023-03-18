using RST.AspNetCore.Extensions.Contracts;

namespace RST.AspNetCore.Extensions.Defaults;

/// <summary>
/// 
/// </summary>
public record DefaultApplicationAuthenticationToken : IApplicationAuthenticationToken
{
    /// <summary>
    /// 
    /// </summary>
    public string? AuthorisationToken { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? ETag { get; set; }
}
