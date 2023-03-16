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
        private readonly IDecryptor decryptor;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <param name="decryptor"></param>
        /// <param name="applicationAuthenticationRepository"></param>
        public ApplicationAuthentication(IOptionsMonitor<ApplicationAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            IDecryptor decryptor, IApplicationAuthenticationRepository applicationAuthenticationRepository) : base(options, logger, encoder, clock)
        {
            this.decryptor = decryptor;
            this.applicationAuthenticationRepository = applicationAuthenticationRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Task<AuthenticateResult> HandleError(Exception exception)
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

                var publicKey = decryptor.Decrypt(encryptedGlob, (Options.EncryptionOptions ?? throw new NullReferenceException("Unable to decrypt glob")).CreateInstance(globPublicKey));

                var identity = await applicationAuthenticationRepository.GetIdentity(publicKey);

                if (identity == null)
                {
                    throw new NullReferenceException("Identity not found");
                }
                
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
