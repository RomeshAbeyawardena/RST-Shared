using Microsoft.AspNetCore.Authentication;
using RST.Contracts;

namespace RST.AspNetCore.Extensions;

/// <summary>
/// 
/// </summary>
public class ApplicationAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public ApplicationAuthenticationSchemeOptions()
        : this(string.Empty)
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public ApplicationAuthenticationSchemeOptions(string scheme)
    {
        Scheme = scheme;
    }

    /// <summary>
    /// 
    /// </summary>
    public string Scheme { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IEncryptionOptions? EncryptionOptions { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public override void Validate()
    {
        base.Validate();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scheme"></param>
    public override void Validate(string scheme)
    {
        base.Validate(scheme);
    }
}
