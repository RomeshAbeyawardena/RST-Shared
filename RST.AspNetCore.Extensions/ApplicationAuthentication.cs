using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RST.AspNetCore.Extensions.Contracts;
using RST.Contracts;
using System.Buffers.Text;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace RST.AspNetCore.Extensions
{
    internal class ApplicationAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public ApplicationAuthenticationSchemeOptions()
        {

        }

        public Type IdentityType { get; set; }

        public override void Validate()
        {
            base.Validate();
        }

        public override void Validate(string scheme)
        {
            base.Validate(scheme);
        }
    }
    internal class ApplicationAuthentication : AuthenticationHandler<ApplicationAuthenticationSchemeOptions>
    {
        private readonly IApplicationAuthenticationRepository applicationAuthenticationRepository;

        public ApplicationAuthentication(IOptionsMonitor<ApplicationAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            ISecuritySignatureProvider securitySignatureProvider, IApplicationAuthenticationRepository applicationAuthenticationRepository) : base(options, logger, encoder, clock)
        {
            this.applicationAuthenticationRepository = applicationAuthenticationRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorisationToken = Request.Headers.Authorization.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authorisationToken) || authorisationToken.Length < 32)
            {
                return AuthenticateResult.Fail(new NullReferenceException());
            }

            var publicKey = authorisationToken.Substring(authorisationToken.Length - 16, 16);

            var identity = await applicationAuthenticationRepository.GetIdentity(publicKey);

            if (identity == null)
            {
                return AuthenticateResult.Fail(new NullReferenceException());
            }

            var roles = await applicationAuthenticationRepository.GetRoles(identity);

            if (roles == null)
            {
                return AuthenticateResult.Fail(new NullReferenceException());
            }

            var claims = roles.Select(s => new Claim(s.Key, s.Value));

            return AuthenticateResult.Success(
                new AuthenticationTicket(new ClaimsPrincipal(
                    new ClaimsIdentity(identity, claims)), ""));
        }
    }
}
