using RST.AspNetCore.Extensions.Contracts;
using RST.Contracts;

namespace RST.AspNetCore.Extensions.Defaults;

/// <summary>
/// 
/// </summary>
public class DefaultApplicationAuthenticationTokenBuilder : IApplicationAuthenticationTokenBuilder
{
    private readonly IEncryptionModuleOptions encryptionModuleOptions;
    private readonly IEncryptor encryptor;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptionModuleOptions"></param>
    /// <param name="encryptor"></param>
    public DefaultApplicationAuthenticationTokenBuilder(IEncryptionModuleOptions encryptionModuleOptions, IEncryptor encryptor)
    {
        this.encryptionModuleOptions = encryptionModuleOptions;
        this.encryptor = encryptor;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="applicationIdentity"></param>
    /// <param name="encryptionOptions"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string BuildToken(IApplicationIdentity applicationIdentity, IEncryptionOptions? encryptionOptions)
    {
        return BuildToken(applicationIdentity.PublicKey, encryptionOptions);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="publicKey"></param>
    /// <param name="encryptionOptions"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string BuildToken(string publicKey, IEncryptionOptions? encryptionOptions)
    {
        if(encryptionOptions == null)
        {
            encryptionOptions = encryptionModuleOptions["" ?? string.Empty] ?? throw new NullReferenceException();
        }

        return string.Empty;
    }
}
