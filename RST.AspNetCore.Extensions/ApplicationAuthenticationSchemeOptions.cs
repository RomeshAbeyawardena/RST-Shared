using Microsoft.AspNetCore.Authentication;
using RST.Contracts;

namespace RST.AspNetCore.Extensions;

/// <summary>
/// 
/// </summary>
public class ApplicationAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// 
    /// </summary>
    public ApplicationAuthenticationSchemeOptions()
        : this(string.Empty, default)
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="scheme"></param>
    /// <param name="encryptionOptions"></param>
    public ApplicationAuthenticationSchemeOptions(string scheme, IEncryptionOptions? encryptionOptions)
    {
        Scheme = scheme;
        EncryptionOptions = encryptionOptions;
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
        if(string.IsNullOrWhiteSpace(Scheme))
        {
            throw new ArgumentNullException(nameof(Scheme));
        }

        if (EncryptionOptions == null)
        {
            throw new ArgumentNullException(nameof(EncryptionOptions));
        }

        base.Validate();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="scheme"></param>
    public override void Validate(string scheme)
    {
        Scheme = scheme;
        Validate();
        base.Validate(scheme);
    }
}
