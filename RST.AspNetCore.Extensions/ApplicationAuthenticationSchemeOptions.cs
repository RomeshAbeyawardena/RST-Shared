using Microsoft.AspNetCore.Authentication;
using RST.Contracts;

namespace RST.AspNetCore.Extensions;

/// <summary>
/// Represents an implementation of <see cref="AuthenticationSchemeOptions"/>
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
    /// <param name="encryptionKey"></param>
    /// <param name="encryptionOptions"></param>
    public ApplicationAuthenticationSchemeOptions(string scheme,
        string? encryptionKey = null,
        IEncryptionOptions? encryptionOptions = null)
    {
        Scheme = scheme;
        EncryptionOptions = encryptionOptions;
    }

    /// <summary>
    /// Get or sets a value to determine whether authentication handler should verify the signature in the ETag
    /// </summary>
    public bool VerifySignature { get; set; }
    /// <summary>
    /// Gets or sets the scheme
    /// </summary>
    public string Scheme { get; set; }
    /// <summary>
    /// Gets or the sets the encryption key used to obtain <see cref="IEncryptionOptions"/> from <see cref="IEncryptionModuleOptions"/>
    /// </summary>
    public string? EncryptionKey { get; set; }
    /// <summary>
    /// Gets or sets a value to determine whether authenticator handler should use providers encryption options
    /// </summary>
    public bool UseProvidedEncryptionOptionsOnly { get; set; }

    /// <summary>
    /// Gets or sets the encryption options to be used for encrypting and decrypting the public accessible parameters
    /// </summary>
    public IEncryptionOptions? EncryptionOptions { get; set; }

    /// <summary>
    /// <inheritdoc cref="AuthenticationSchemeOptions.Validate()"/>
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    public override void Validate()
    {
        if(string.IsNullOrWhiteSpace(Scheme))
        {
            throw new ArgumentNullException(nameof(Scheme));
        }

        if (UseProvidedEncryptionOptionsOnly && EncryptionOptions == null)
        {
            throw new ArgumentNullException(nameof(EncryptionOptions));
        }

        base.Validate();
    }

    /// <inheritdoc cref="AuthenticationSchemeOptions.Validate(string)"/>
    /// <exception cref="ArgumentNullException"></exception>
    public override void Validate(string scheme)
    {
        Scheme = scheme;
        Validate();
        base.Validate(scheme);
    }
}
