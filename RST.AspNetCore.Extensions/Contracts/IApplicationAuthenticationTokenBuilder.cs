using RST.Contracts;

namespace RST.AspNetCore.Extensions.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationAuthenticationTokenBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationIdentity"></param>
        /// <param name="encryptionOptions"></param>
        /// <returns></returns>
        string BuildToken(IApplicationIdentity applicationIdentity, IEncryptionOptions? encryptionOptions = null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="encryptionOptions"></param>
        /// <returns></returns>
        string BuildToken(string publicKey, IEncryptionOptions? encryptionOptions = null);
    }
}
