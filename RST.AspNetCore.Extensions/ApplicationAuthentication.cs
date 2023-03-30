using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RST.AspNetCore.Extensions.Contracts;
using System.Text.Encodings.Web;

namespace RST.AspNetCore.Extensions
{
    /// <summary>
    /// Represents application authentication
    /// </summary>
    public class ApplicationAuthentication : AuthenticationHandler<ApplicationAuthenticationSchemeOptions>
    {
        private readonly IApplicationAuthenticationProvider applicationAuthenticationProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <param name="applicationAuthenticationProvider"></param>
        public ApplicationAuthentication(IOptionsMonitor<ApplicationAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            IApplicationAuthenticationProvider applicationAuthenticationProvider)
            : base(options, logger, encoder, clock)
        {
            this.applicationAuthenticationProvider = applicationAuthenticationProvider;
        }


        /// <inheritdoc cref="AuthenticationHandler{ApplicationAuthenticationSchemeOptions}.AuthenticateAsync"/>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await applicationAuthenticationProvider.HandleAuthenticateAsync(Options);
        }
    }
}
