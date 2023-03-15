using RST.AspNetCore.Extensions.Contracts;
using RST.Contracts;
using RST.Security.Cryptography.Defaults;

namespace RST.AspNetCore.Extensions
{
    internal abstract class BaseApplicationAuthenticationRepository<TKey, TIdentity, TTimestamp> : IApplicationAuthenticationRepository<TKey, TIdentity, TTimestamp>
        where TIdentity : IIdentity<TKey>
        where TKey : struct
        where TTimestamp : struct
    {
        private readonly ISecuritySignatureProvider securitySignature;
        protected virtual Task<bool> OnValidateIdentitySignature(TIdentity identity, string signature, ISignatureConfiguration configuration)
        {
            return Task.FromResult(true);
        }

        public BaseApplicationAuthenticationRepository(ISecuritySignatureProvider securitySignature)
        {
            this.securitySignature = securitySignature;
        }

        public abstract IApplicationIdentity<TKey, TTimestamp> Convert(TIdentity identity);
        public abstract Task<TIdentity> GetIdentity(TKey key);

        public abstract Task<TIdentity> GetIdentity(string publicKey);
        public abstract Task<IDictionary<string, string>> GetRoles(TIdentity identity);

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

            return securitySignature.VerifyData(applicationIdentity.AccessToken, signature, configuration);
        }
    }
}
