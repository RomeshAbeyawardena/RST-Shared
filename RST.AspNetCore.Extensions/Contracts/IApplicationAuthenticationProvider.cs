using Microsoft.AspNetCore.Authentication;
using RST.Contracts;
using System.Security.Claims;

namespace RST.AspNetCore.Extensions.Contracts;

/// <summary>
/// 
/// </summary>
public interface IApplicationAuthenticationProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="authorisationToken"></param>
    /// <returns></returns>
    Tuple<string, string> ExtractEncryptedGlob(string? authorisationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="globPublicKey"></param>
    /// <returns></returns>
    IEncryptionOptions GetEncryptionOptions(string globPublicKey);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptedSignature"></param>
    /// <param name="encryptionOptions"></param>
    /// <param name="authorisationToken"></param>
    /// <param name="identity"></param>
    /// <returns></returns>
    string VerifySignature(string? encryptedSignature, IEncryptionOptions encryptionOptions, string? authorisationToken, IApplicationIdentity identity);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptedAccessToken"></param>
    /// <param name="encryptionOptions"></param>
    /// <param name="identity"></param>
    /// <returns></returns>
    Task ValidateAccessToken(string? encryptedAccessToken, IEncryptionOptions encryptionOptions, IApplicationIdentity identity);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identity"></param>
    /// <returns></returns>
    Task<IEnumerable<Claim>> GetRoles(IApplicationIdentity identity);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<AuthenticateResult> HandleAuthenticateAsync();
}
