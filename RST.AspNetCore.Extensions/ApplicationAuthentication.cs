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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <param name="securitySignatureProvider"></param>
        /// <param name="applicationAuthenticationRepository"></param>
        public ApplicationAuthentication(IOptionsMonitor<ApplicationAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            ISecuritySignatureProvider securitySignatureProvider, IApplicationAuthenticationRepository applicationAuthenticationRepository) : base(options, logger, encoder, clock)
        {
            this.applicationAuthenticationRepository = applicationAuthenticationRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var authorisationToken = Request.Headers.Authorization.FirstOrDefault();
                if (string.IsNullOrWhiteSpace(authorisationToken) || authorisationToken.Length < 32)
                {
                    throw new NullReferenceException();
                }

                var publicKey = authorisationToken.Substring(authorisationToken.Length - 16, 16);

                var identity = await applicationAuthenticationRepository.GetIdentity(publicKey);

                if (identity == null)
                {
                    throw new NullReferenceException();
                }

                var roles = await applicationAuthenticationRepository.GetRoles(identity);

                if (roles == null)
                {
                    throw new NullReferenceException();
                }

                var claims = roles.Select(s => new Claim(s.Key, s.Value));

                return AuthenticateResult.Success(
                    new AuthenticationTicket(new ClaimsPrincipal(
                        new ClaimsIdentity(identity, claims)), ""));
            }
            catch(NullReferenceException ex)
            {
                return AuthenticateResult.Fail(ex);
            }
        }
    }
}
