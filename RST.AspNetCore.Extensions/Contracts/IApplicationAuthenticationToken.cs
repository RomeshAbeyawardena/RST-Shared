namespace RST.AspNetCore.Extensions.Contracts;

/// <summary>
/// 
/// </summary>
public interface IApplicationAuthenticationToken
{
    /// <summary>
    /// 
    /// </summary>
    string? AuthorisationToken { get; set; }
    /// <summary>
    /// 
    /// </summary>
    string? ETag { get; set; }
}
