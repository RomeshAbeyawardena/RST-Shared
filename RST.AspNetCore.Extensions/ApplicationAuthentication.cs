using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RST.AspNetCore.Extensions.Contracts;
using RST.Contracts;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace RST.AspNetCore.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationAuthentication : AuthenticationHandler<ApplicationAuthenticationSchemeOptions>
    {
        private readonly IApplicationAuthenticationRepository applicationAuthenticationRepository;
        private readonly IEncryptionModuleOptions encryptionModuleOptions;
        private readonly IDecryptor decryptor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <param name="encryptionModuleOptions"></param>
        /// <param name="decryptor"></param>
        /// <param name="applicationAuthenticationRepository"></param>
        public ApplicationAuthentication(IOptionsMonitor<ApplicationAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IEncryptionModuleOptions encryptionModuleOptions,
            IDecryptor decryptor, 
            IApplicationAuthenticationRepository applicationAuthenticationRepository) : base(options, logger, encoder, clock)
        {
            this.encryptionModuleOptions = encryptionModuleOptions;
            this.decryptor = decryptor;
            this.applicationAuthenticationRepository = applicationAuthenticationRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            static Task<AuthenticateResult> HandleError(Exception exception)
            {
                return Task.FromResult(AuthenticateResult.Fail(exception));
            }

            try
            {
                var authorisationToken = Request.Headers.Authorization.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(authorisationToken) || authorisationToken.Length < 132)
                {
                    throw new NullReferenceException("Token invalid");
                }

                var globPublicKey = authorisationToken.Substring(authorisationToken.Length - 16, 16);

                var encryptedGlob = authorisationToken[..^16];

                var encryptionOptions = (Options.EncryptionOptions ?? encryptionModuleOptions[string.Empty] ?? throw new NullReferenceException("Unable to decrypt glob")).CreateInstance(globPublicKey);

                var publicKey = decryptor.Decrypt(encryptedGlob, encryptionOptions);

                var identity = await applicationAuthenticationRepository.GetIdentity(publicKey);

                if (identity == null)
                {
                    throw new NullReferenceException("Identity not found");
                }

                var encryptedAccessToken = Request.Headers.ETag.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(encryptedAccessToken))
                {
                    throw new NullReferenceException("Access token invalid");
                }

                var accessToken = decryptor.Decrypt(encryptedAccessToken, encryptionOptions);

                var roles = await applicationAuthenticationRepository.GetRoles(identity);

                if (roles == null)
                {
                    throw new NullReferenceException("Identity has no roles");
                }

                var claims = roles.Select(s => new Claim(s.Key, s.Value));

                return AuthenticateResult.Success(
                    new AuthenticationTicket(new ClaimsPrincipal(
                        new ClaimsIdentity(identity, claims)), Options.Scheme));
            }
            catch(NullReferenceException ex)
            {
                return await HandleError(ex);
            }
        }
    }
}
