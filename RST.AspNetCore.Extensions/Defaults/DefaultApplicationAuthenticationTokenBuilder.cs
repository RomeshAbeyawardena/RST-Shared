using RST.AspNetCore.Extensions.Contracts;
using RST.Contracts;
using RST.Security.Cryptography.Defaults;
using System.Security.Principal;

namespace RST.AspNetCore.Extensions.Defaults;

/// <summary>
/// 
/// </summary>
public class DefaultApplicationAuthenticationTokenBuilder : IApplicationAuthenticationTokenBuilder
{
    private readonly IEncryptionModuleOptions encryptionModuleOptions;
    private readonly IEncryptor encryptor;
    private readonly IApplicationAuthenticationProvider applicationAuthenticationProvider;
    private readonly ISecuritySignatureProvider securitySignatureProvider;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptionModuleOptions"></param>
    /// <param name="encryptor"></param>
    /// <param name="applicationAuthenticationProvider"></param>
    /// <param name="securitySignatureProvider"></param>
    public DefaultApplicationAuthenticationTokenBuilder(IEncryptionModuleOptions encryptionModuleOptions, IEncryptor encryptor, IApplicationAuthenticationProvider applicationAuthenticationProvider,
        ISecuritySignatureProvider securitySignatureProvider)
    {
        this.encryptionModuleOptions = encryptionModuleOptions;
        this.encryptor = encryptor;
        this.applicationAuthenticationProvider = applicationAuthenticationProvider;
        this.securitySignatureProvider = securitySignatureProvider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationIdentity"></param>
    /// <param name="publicKey"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IApplicationAuthenticationToken BuildToken(IApplicationIdentity applicationIdentity, 
        string publicKey, 
        ApplicationAuthenticationSchemeOptions options)
    {
        var applicationAuthenticationToken = new DefaultApplicationAuthenticationToken();

        var encryptionOptions = applicationAuthenticationProvider
            .GetEncryptionOptions(publicKey, options);

        var encryptedGlob = encryptor.Encrypt(applicationIdentity.PublicKey, encryptionOptions);

        applicationAuthenticationToken.AuthorisationToken = $"{encryptedGlob}{publicKey}";

        applicationAuthenticationToken.ETag = encryptor.Encrypt(securitySignatureProvider.SignData(applicationAuthenticationToken.AuthorisationToken, DefaultSignatureConfiguration.DefaultConfiguration(applicationIdentity.PublicKey)), encryptionOptions);

        return applicationAuthenticationToken;
    }
}
