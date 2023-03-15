using Microsoft.Extensions.DependencyInjection;
using RST.Attributes;
using RST.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace RST.Security.Cryptography.Defaults;

/// <summary>
/// 
/// </summary>
[Register(ServiceLifetime.Transient)]
public class DefaultSecuritySignatureProvider : ISecuritySignatureProvider
{
    private readonly IServiceProvider serviceProvider;
    private Lazy<RSA> lazyRsa;
    private RSA Rsa => lazyRsa.Value;
    private static Encoding GetEncodingOrDefault(ISignatureConfiguration configuration)
    {
        return configuration.Encoding ?? Encoding.Default;
    }

    private void ImportKeys(ISignatureConfiguration configuration)
    {
        if (!string.IsNullOrEmpty(configuration.PublicKey))
        {
            var publicKeyBytes = Convert.FromBase64String(configuration.PublicKey);
            Rsa?.ImportRSAPublicKey(publicKeyBytes, out var ct);
        }

        if (!string.IsNullOrEmpty(configuration.PrivateKey))
        {
            var privateKeyBytes = Convert.FromBase64String(configuration.PrivateKey);
            Rsa!.ImportEncryptedPkcs8PrivateKey(configuration.PrivateKeyPassword, privateKeyBytes, out var ct1);
        }
    }

    internal void FlushProvider()
    {
        Rsa?.Dispose();
        lazyRsa = new Lazy<RSA>(serviceProvider.GetRequiredService<RSA>);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    public DefaultSecuritySignatureProvider(IServiceProvider serviceProvider)
    {
        lazyRsa = new Lazy<RSA>(serviceProvider.GetRequiredService<RSA>);
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Verifies data using a public key
    /// </summary>
    /// <param name="data"></param>
    /// <param name="signature"></param>
    /// <param name="signatureConfiguration"></param>
    /// <returns></returns>
    public bool VerifyData(string data, string signature, ISignatureConfiguration signatureConfiguration)
    {
        var publicKeyBytes = Convert.FromBase64String(signatureConfiguration.PublicKey ?? throw new ArgumentException());
        Rsa!.ImportRSAPublicKey(publicKeyBytes, out var bytes);
        
        var result = Rsa.VerifyData((signatureConfiguration.Encoding ?? Encoding.Default).GetBytes(data), 
            Convert.FromBase64String(signature), signatureConfiguration.HashAlgorithmName, signatureConfiguration.Padding ?? RSASignaturePadding.Pkcs1);
        FlushProvider();

        return result;
    }

    /// <summary>
    /// Signs data using supplied <paramref name="signatureConfiguration"/>
    /// </summary>
    /// <param name="data"></param>
    /// <param name="signatureConfiguration"></param>
    /// <returns></returns>
    public string SignData(string data, ISignatureConfiguration signatureConfiguration)
    {
        ImportKeys(signatureConfiguration);

        var signature = Rsa!.SignData(Encoding.UTF8.GetBytes(data), signatureConfiguration.HashAlgorithmName, signatureConfiguration.Padding ?? RSASignaturePadding.Pkcs1);
        FlushProvider();
        
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// </summary>
    public void Dispose()
    {
        Rsa?.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Creates an instance of <see cref="ISignatureConfiguration"/> for the an instance of RSA using the parameters in <paramref name="signatureConfiguration"/>
    /// </summary>
    /// <param name="signatureConfiguration"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ISignatureConfiguration CreateConfiguration(ISignatureConfiguration signatureConfiguration)
    {
        
        var publicKeyBytes = Rsa!.ExportRSAPublicKey();
        
        var privateKeyBytes = Rsa.ExportEncryptedPkcs8PrivateKey(signatureConfiguration.PrivateKeyPassword, new PbeParameters(signatureConfiguration.EncryptionAlgorithm, signatureConfiguration.HashAlgorithmName, signatureConfiguration.IterationCount));
        
        var configuration = DefaultSignatureConfiguration.DefaultConfiguration(Convert.ToBase64String(publicKeyBytes), Convert.ToBase64String(privateKeyBytes));
        
        return configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="encryptedValue"></param>
    /// <param name="signatureConfiguration"></param>
    /// <param name="usePublicKey"></param>
    /// <returns></returns>
    public string Decrypt(string encryptedValue, ISignatureConfiguration signatureConfiguration, bool usePublicKey)
    {
        ImportKeys(signatureConfiguration);
        var encodedData = Convert.FromBase64String(encryptedValue);

        string result;
        if(usePublicKey)
        {
            result = GetEncodingOrDefault(signatureConfiguration).GetString(Rsa!.DecryptValue(encodedData));
        }
        else
            result = GetEncodingOrDefault(signatureConfiguration).GetString(Rsa!.Decrypt(encodedData, signatureConfiguration.EncryptionPadding ?? RSAEncryptionPadding.Pkcs1));

        FlushProvider();

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plaintext"></param>
    /// <param name="signatureConfiguration"></param>
    /// <param name="usePublicKey"></param>
    /// <returns></returns>
    public string Encrypt(string plaintext, ISignatureConfiguration signatureConfiguration, bool usePublicKey)
    {
        ImportKeys(signatureConfiguration);
        var encodedData = GetEncodingOrDefault(signatureConfiguration).GetBytes(plaintext);

        string result;
        if (usePublicKey)
        {
            result = Convert.ToBase64String(Rsa!.EncryptValue(encodedData));
        }
        else
            result = Convert.ToBase64String(Rsa!.Encrypt(encodedData, signatureConfiguration.EncryptionPadding ?? RSAEncryptionPadding.Pkcs1));

        FlushProvider();

        return result;
    }
}
