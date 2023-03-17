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
        /// <inheritdoc cref="IApplicationAuthenticationRepository.GetIdentity(string)"/>
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
        /// 
        /// </summary>
        /// <param name="applicationIdentity"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public abstract Task<bool> ValidateAccessToken(IApplicationIdentity<TKey, TTimestamp> applicationIdentity, string accessToken);

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

        async Task<IApplicationIdentity> IApplicationAuthenticationRepository.GetIdentity(string publicKey)
        {
            return Convert(await GetIdentity(publicKey));
        }

        Task<bool> IApplicationAuthenticationRepository.ValidateIdentitySignature(object identity, string signature, ISignatureConfiguration? configuration)
        {
            return ValidateIdentitySignature((TIdentity)identity, signature, configuration);
        }

        Task<IDictionary<string, string>> IApplicationAuthenticationRepository.GetRoles(IApplicationIdentity identity)
        {
            return GetRoles((TIdentity)identity);
        }

        async Task<IApplicationIdentity> IApplicationAuthenticationRepository.GetIdentity(object key)
        {
            return Convert(await GetIdentity((TKey)key));
        }

        Task<bool> IApplicationAuthenticationRepository.ValidateAccessToken(IApplicationIdentity applicationIdentity, string accessToken)
        {
            return ValidateAccessToken((IApplicationIdentity<TKey, TTimestamp>)applicationIdentity, accessToken);
        }
    }
}
