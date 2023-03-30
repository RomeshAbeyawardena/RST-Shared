namespace RST.AspNetCore.Extensions.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationAuthenticationTokenBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationIdentity"></param>
        /// <param name="publicKey"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IApplicationAuthenticationToken BuildToken(IApplicationIdentity applicationIdentity,
            string publicKey,
            ApplicationAuthenticationSchemeOptions options);
    }
}
