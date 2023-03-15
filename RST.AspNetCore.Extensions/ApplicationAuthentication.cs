using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RST.AspNetCore.Extensions.Contracts;
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
        private readonly Contracts.IApplicationAuthenticationRepository<> applicationAuthenticationRepository;

        public ApplicationAuthentication(IOptionsMonitor<ApplicationAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            IServiceProvider serviceProvider) : base(options, logger, encoder, clock)
        {
            this.applicationAuthenticationRepository = applicationAuthenticationRepository;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            applicationAuthenticationRepository.
        }
    }
}
