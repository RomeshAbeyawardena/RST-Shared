using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RST.AspNetCore.Extensions.Contracts;
using RST.Contracts;
using RST.Security.Cryptography.Defaults;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;

namespace RST.AspNetCore.Extensions
{
    /// <summary>
    /// Represents application authentication
    /// </summary>
    public class ApplicationAuthentication : AuthenticationHandler<ApplicationAuthenticationSchemeOptions>
    {
        private readonly IApplicationAuthenticationRepository applicationAuthenticationRepository;
        private readonly IEncryptionModuleOptions encryptionModuleOptions;
        private readonly IDecryptor decryptor;
        private readonly ISecuritySignatureProvider securitySignatureProvider;

        private static Tuple<string, string> ExtractEncryptedGlob(string? authorisationToken)
        {
            if (string.IsNullOrWhiteSpace(authorisationToken) || authorisationToken.Length < 132)
            {
                throw new NullReferenceException("Authorision Token invalid");
            }

            var globPublicKey = authorisationToken.Substring(authorisationToken.Length - 16, 16);

            var encryptedGlob = authorisationToken[..^16];

            return Tuple.Create(globPublicKey, encryptedGlob);
        }

        private IEncryptionOptions GetEncryptionOptions(string globPublicKey)
        {
            return (Options.EncryptionOptions
                    ?? (string.IsNullOrEmpty(Options.EncryptionKey) ? null : encryptionModuleOptions[Options.EncryptionKey])
                    ?? throw new NullReferenceException("Unable to decrypt glob")).CreateInstance(globPublicKey);
        }

        private string VerifySignature(string? encryptedSignature, IEncryptionOptions encryptionOptions, string? authorisationToken, IApplicationIdentity identity)
        {
            if (string.IsNullOrWhiteSpace(encryptedSignature))
            {
                throw new NullReferenceException("Signature invalid");
            }

            if (string.IsNullOrWhiteSpace(authorisationToken))
            {
                throw new NullReferenceException("Authorisation token invalid invalid");
            }

            var signature = decryptor.Decrypt(encryptedSignature, encryptionOptions);

            if (!securitySignatureProvider.VerifyData(authorisationToken, signature, DefaultSignatureConfiguration.DefaultConfiguration(identity.PublicKey)))
            {
                throw new InvalidOperationException("ETag signature invalid");
            }

            return signature;
        }

        private async Task ValidateAccessToken(string? encryptedAccessToken, IEncryptionOptions encryptionOptions, IApplicationIdentity identity)
        {
            if (string.IsNullOrWhiteSpace(encryptedAccessToken))
            {
                throw new NullReferenceException("Access token invalid");
            }

            var accessToken = decryptor.Decrypt(encryptedAccessToken, encryptionOptions);

            if (!await applicationAuthenticationRepository.ValidateAccessToken(identity, accessToken))
            {
                throw new InvalidOperationException("Access token invalid");
            }
        }

        private async Task<IEnumerable<Claim>> GetRoles(IApplicationIdentity identity)
        {
            var roles = await applicationAuthenticationRepository.GetRoles(identity);

            if (roles == null)
            {
                throw new NullReferenceException("Identity has no roles");
            }

            return roles.Select(s => new Claim(s.Key, s.Value));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <param name="encryptionModuleOptions"></param>
        /// <param name="decryptor"></param>
        /// <param name="securitySignatureProvider"></param>
        /// <param name="applicationAuthenticationRepository"></param>
        public ApplicationAuthentication(IOptionsMonitor<ApplicationAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IEncryptionModuleOptions encryptionModuleOptions,
            IDecryptor decryptor, ISecuritySignatureProvider securitySignatureProvider,
            IApplicationAuthenticationRepository applicationAuthenticationRepository) : base(options, logger, encoder, clock)
        {
            this.encryptionModuleOptions = encryptionModuleOptions;
            this.decryptor = decryptor;
            this.securitySignatureProvider = securitySignatureProvider;
            this.applicationAuthenticationRepository = applicationAuthenticationRepository;
        }

        
        /// <inheritdoc cref="AuthenticationHandler{ApplicationAuthenticationSchemeOptions}.AuthenticateAsync"/>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            static Task<AuthenticateResult> HandleError(Exception exception)
            {
                return Task.FromResult(AuthenticateResult.Fail(exception));
            }

            try
            {
                var authorisationToken = Request.Headers.Authorization.FirstOrDefault();
                var (globPublicKey, encryptedGlob) = ExtractEncryptedGlob(authorisationToken);

                var encryptionOptions = GetEncryptionOptions(globPublicKey);

                var publicKey = decryptor.Decrypt(encryptedGlob, encryptionOptions);

                var identity = await applicationAuthenticationRepository.GetIdentity(publicKey) ?? throw new NullReferenceException("Identity not found");

                if (Options.VerifySignature)
                {
                    var encryptedSignature = Request.Headers.ETag.FirstOrDefault();

                    VerifySignature(encryptedSignature, encryptionOptions, authorisationToken, identity);
                }

                var encryptedAccessToken = Request.Headers.WWWAuthenticate.FirstOrDefault();
                await ValidateAccessToken(encryptedAccessToken, encryptionOptions, identity);

                var claims = await GetRoles(identity);

                return AuthenticateResult.Success(
                    new AuthenticationTicket(new ClaimsPrincipal(
                        new ClaimsIdentity(identity, claims)), Options.Scheme));
            }
            catch(ArgumentOutOfRangeException ex)
            {
                Logger.LogError(ex, "Authorision token invalid");
                return await HandleError(new InvalidDataException("Authorision token invalid"));
            }
            catch(InvalidOperationException ex)
            {
                return await HandleError(ex);
            }
            catch(NullReferenceException ex)
            {
                return await HandleError(ex);
            }
        }
    }
}
