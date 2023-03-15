using RST.AspNetCore.Extensions.Contracts;
using RST.Contracts;
using RST.Security.Cryptography.Defaults;

namespace RST.AspNetCore.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TIdentity"></typeparam>
    /// <typeparam name="TTimestamp"></typeparam>
    public abstract class BaseApplicationAuthenticationRepository<TKey, TIdentity, TTimestamp> : IApplicationAuthenticationRepository<TKey, TIdentity, TTimestamp>
        where TIdentity : IIdentity<TKey>
        where TKey : struct
        where TTimestamp : struct
    {
        private readonly ISecuritySignatureProvider securitySignatureProvider;
        /// <summary>
        /// <inheritdoc cref="IApplicationAuthenticationRepository{TKey, TIdentity, TTimestamp}.ValidateIdentitySignature(TIdentity, string, ISignatureConfiguration?)" />
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="signature"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        protected virtual Task<bool> OnValidateIdentitySignature(TIdentity identity, string signature, ISignatureConfiguration configuration)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="securitySignature"></param>
        public BaseApplicationAuthenticationRepository(ISecuritySignatureProvider securitySignature)
        {
            this.securitySignatureProvider = securitySignature;
        }

        /// <summary>
        /// <inheritdoc cref="IApplicationAuthenticationRepository{TKey, TIdentity, TTimestamp}.Convert(TIdentity)"/>
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public abstract IApplicationIdentity<TKey, TTimestamp> Convert(TIdentity identity);
        /// <summary>
        /// <inheritdoc cref="IApplicationAuthenticationRepository{TKey, TIdentity, TTimestamp}.GetIdentity(TKey)"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract Task<TIdentity> GetIdentity(TKey key);
        /// <summary>
        /// <inheritdoc cref="IApplicationAuthenticationRepository{TKey, TIdentity, TTimestamp}.GetIdentity(string)"/>
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        public abstract Task<TIdentity> GetIdentity(string publicKey);
        /// <summary>
        /// <inheritdoc cref="IApplicationAuthenticationRepository{TKey, TIdentity, TTimestamp}.GetRoles(TIdentity)"/>
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public abstract Task<IDictionary<string, string>> GetRoles(TIdentity identity);

        /// <summary>
        /// <inheritdoc cref="IApplicationAuthenticationRepository{TKey, TIdentity, TTimestamp}.ValidateIdentitySignature(TIdentity, string, ISignatureConfiguration?)"/>
        /// or logic in <see cref="BaseApplicationAuthenticationRepository{TKey, TIdentity, TTimestamp}.OnValidateIdentitySignature(TIdentity, string, ISignatureConfiguration)"/>
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="signature"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public virtual async Task<bool> ValidateIdentitySignature(TIdentity identity, string signature, ISignatureConfiguration? configuration = null)
        {
            var applicationIdentity = Convert(identity);
            
            if(applicationIdentity == null || !applicationIdentity.IsEnabled)
            {
                return false;
            }

            configuration ??= DefaultSignatureConfiguration.DefaultConfiguration(applicationIdentity.PublicKey);

            if(!await OnValidateIdentitySignature(identity, signature, configuration))
            {
                return false;
            }

            return securitySignatureProvider.VerifyData(applicationIdentity.AccessToken, signature, configuration);
        }
    }
}
