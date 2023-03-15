using Microsoft.AspNetCore.Authentication;

namespace RST.AspNetCore.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public ApplicationAuthenticationSchemeOptions()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void Validate()
        {
            base.Validate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheme"></param>
        public override void Validate(string scheme)
        {
            base.Validate(scheme);
        }
    }
}
