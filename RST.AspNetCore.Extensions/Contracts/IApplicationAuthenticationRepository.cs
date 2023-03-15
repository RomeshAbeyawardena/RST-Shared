using RST.Contracts;

namespace RST.AspNetCore.Extensions.Contracts
{
    /// <summary>
    /// Represents an application authentication repository
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TIdentity"></typeparam>
    /// <typeparam name="TTimestamp"></typeparam>
    public interface IApplicationAuthenticationRepository<TKey, TIdentity, TTimestamp>
        where TIdentity : IIdentity<TKey>
        where TKey : struct
        where TTimestamp : struct
    {
        /// <summary>
        /// Converts a <typeparamref name="TIdentity"/> to an instance of <see cref="IApplicationIdentity{TKey, TTimeStamp}"/>
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        IApplicationIdentity<TKey, TTimestamp> Convert(TIdentity identity);

        /// <summary>
        /// Gets an identity by its unique <typeparamref name="TKey"/> identifier
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<TIdentity> GetIdentity(TKey key);

        /// <summary>
        /// Gets an identity by its public key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        Task <TIdentity> GetIdentity(string publicKey);

        /// <summary>
        /// Validates a signature based on the the <see cref="IApplicationIdentity{TKey, TTimeStamp}"/> Access token value
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="signature"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task<bool> ValidateIdentitySignature(TIdentity identity, string signature, ISignatureConfiguration? configuration = null);
        /// <summary>
        /// Gets roles for the application identity
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        Task<IDictionary<string, string>> GetRoles(TIdentity identity);
    }
}
