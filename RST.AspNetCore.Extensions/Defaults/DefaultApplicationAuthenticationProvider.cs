using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RST.AspNetCore.Extensions.Contracts;
using RST.Contracts;
using RST.Security.Cryptography.Defaults;
using System.Security.Claims;

namespace RST.AspNetCore.Extensions.Defaults;

/// <inheritdoc cref="IApplicationAuthenticationProvider"/>
public class DefaultApplicationAuthenticationProvider : IApplicationAuthenticationProvider
{
    private readonly ILogger<DefaultApplicationAuthenticationProvider> logger;
    private readonly IEncryptionModuleOptions encryptionModuleOptions;
    private readonly IHeaderDictionary headers;
    private readonly IDecryptor decryptor;
    private readonly ISecuritySignatureProvider securitySignatureProvider;
    private readonly IApplicationAuthenticationRepository applicationAuthenticationRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptionModuleOptions"></param>
    /// <param name="logger"></param>
    /// <param name="headers"></param>
    /// <param name="decryptor"></param>
    /// <param name="securitySignatureProvider"></param>
    /// <param name="applicationAuthenticationRepository"></param>
    public DefaultApplicationAuthenticationProvider(
            ILogger<DefaultApplicationAuthenticationProvider> logger,
            IEncryptionModuleOptions encryptionModuleOptions, IHeaderDictionary headers,
            IDecryptor decryptor, ISecuritySignatureProvider securitySignatureProvider,
            IApplicationAuthenticationRepository applicationAuthenticationRepository)
    {
        this.logger = logger;
        this.encryptionModuleOptions = encryptionModuleOptions;
        this.headers = headers;
        this.decryptor = decryptor;
        this.securitySignatureProvider = securitySignatureProvider;
        this.applicationAuthenticationRepository = applicationAuthenticationRepository;
    }

    /// <inheritdoc cref="IApplicationAuthenticationProvider.ExtractEncryptedGlob(string?)"/>
    public Tuple<string, string> ExtractEncryptedGlob(string? authorisationToken)
    {
        if (string.IsNullOrWhiteSpace(authorisationToken) || authorisationToken.Length < 132)
        {
            throw new NullReferenceException("Authorision Token invalid");
        }

        var globPublicKey = authorisationToken.Substring(authorisationToken.Length - 16, 16);

        var encryptedGlob = authorisationToken[..^16];

        return Tuple.Create(globPublicKey, encryptedGlob);
    }

    /// <inheritdoc cref="IApplicationAuthenticationProvider.GetEncryptionOptions(string, ApplicationAuthenticationSchemeOptions)"/>
    public IEncryptionOptions GetEncryptionOptions(string globPublicKey, ApplicationAuthenticationSchemeOptions options)
    {
        return (options.EncryptionOptions
                    ?? (string.IsNullOrEmpty(options.EncryptionKey) ? null : encryptionModuleOptions[options.EncryptionKey])
                    ?? throw new NullReferenceException("Unable to decrypt glob")).CreateInstance(globPublicKey);
    }
    /// <inheritdoc cref="IApplicationAuthenticationProvider.GetRoles(IApplicationIdentity)"/>
    public async Task<IEnumerable<Claim>> GetRoles(IApplicationIdentity identity)
    {
        var roles = await applicationAuthenticationRepository.GetRoles(identity);

        if (roles == null)
        {
            throw new NullReferenceException("Identity has no roles");
        }

        return roles.Select(s => new Claim(s.Key, s.Value));
    }
    /// <inheritdoc cref="IApplicationAuthenticationProvider.HandleAuthenticateAsync(ApplicationAuthenticationSchemeOptions)"/>
    public async Task<AuthenticateResult> HandleAuthenticateAsync(ApplicationAuthenticationSchemeOptions options)
    {
        static Task<AuthenticateResult> HandleError(Exception exception)
        {
            return Task.FromResult(AuthenticateResult.Fail(exception));
        }

        try
        {
            var authorisationToken = headers.Authorization.FirstOrDefault();
            var (globPublicKey, encryptedGlob) = ExtractEncryptedGlob(authorisationToken);

            var encryptionOptions = GetEncryptionOptions(globPublicKey, options);

            var publicKey = decryptor.Decrypt(encryptedGlob, encryptionOptions);

            var identity = await applicationAuthenticationRepository.GetIdentity(publicKey) ?? throw new NullReferenceException("Identity not found");

            if (options.VerifySignature)
            {
                var encryptedSignature = headers.ETag.FirstOrDefault();

                VerifySignature(encryptedSignature, encryptionOptions, authorisationToken, identity);
            }

            var encryptedAccessToken = headers.WWWAuthenticate.FirstOrDefault();
            await ValidateAccessToken(encryptedAccessToken, encryptionOptions, identity);

            var claims = await GetRoles(identity);

            return AuthenticateResult.Success(
                new AuthenticationTicket(new ClaimsPrincipal(
                    new ClaimsIdentity(identity, claims)), options.Scheme));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            logger.LogError(ex, "Authorision token invalid");
            return await HandleError(new InvalidDataException("Authorision token invalid"));
        }
        catch (InvalidOperationException ex)
        {
            return await HandleError(ex);
        }
        catch (NullReferenceException ex)
        {
            return await HandleError(ex);
        }
    }

    /// <inheritdoc cref="IApplicationAuthenticationProvider.ValidateAccessToken(string?, IEncryptionOptions, IApplicationIdentity)"/>
    public async Task ValidateAccessToken(string? encryptedAccessToken, IEncryptionOptions encryptionOptions, IApplicationIdentity identity)
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

    /// <inheritdoc cref="IApplicationAuthenticationProvider.VerifySignature(string?, IEncryptionOptions, string?, IApplicationIdentity)"/>
    public string VerifySignature(string? encryptedSignature, IEncryptionOptions encryptionOptions, string? authorisationToken, IApplicationIdentity identity)
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
}
