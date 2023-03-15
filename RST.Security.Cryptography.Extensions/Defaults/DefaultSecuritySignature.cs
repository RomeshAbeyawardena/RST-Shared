using Microsoft.Extensions.DependencyInjection;
using RST.Attributes;
using RST.Contracts;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace RST.Security.Cryptography.Defaults;

/// <summary>
/// 
/// </summary>
[Register(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
public class DefaultSecuritySignature : ISecuritySignature
{
    private RSA? rSA;
    private readonly IServiceProvider serviceProvider;

    private Encoding GetEncodingOrDefault(ISignatureConfiguration configuration)
    {
        return configuration.Encoding ?? Encoding.Default;
    }

    private void ImportKeys(ISignatureConfiguration configuration)
    {
        if (!string.IsNullOrEmpty(configuration.PublicKey))
        {
            var publicKeyBytes = Convert.FromBase64String(configuration.PublicKey);
            rSA?.ImportRSAPublicKey(publicKeyBytes, out var ct);
        }

        var privateKeyBytes = rSA!.ExportEncryptedPkcs8PrivateKey(configuration.PrivateKeyPassword, new PbeParameters(configuration.EncryptionAlgorithm, configuration.HashAlgorithmName, configuration.IterationCount));
        rSA!.ImportEncryptedPkcs8PrivateKey(configuration.PrivateKeyPassword, privateKeyBytes, out var ct1);
    }

    internal void FlushProvider()
    {
        rSA?.Dispose();
        rSA = serviceProvider.GetRequiredService<RSA>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    public DefaultSecuritySignature(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        FlushProvider();
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
        rSA!.ImportRSAPublicKey(publicKeyBytes, out var bytes);
        
        return rSA.VerifyData((signatureConfiguration.Encoding ?? Encoding.Default).GetBytes(data), 
            Convert.FromBase64String(signature), signatureConfiguration.HashAlgorithmName, signatureConfiguration.Padding ?? RSASignaturePadding.Pkcs1);
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

        var signature = rSA!.SignData(Encoding.UTF8.GetBytes(data), signatureConfiguration.HashAlgorithmName, signatureConfiguration.Padding ?? RSASignaturePadding.Pkcs1);

        //Debug.WriteLine(Convert.ToBase64String(signature), "Signature");
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// <inheritdoc cref="IDisposable.Dispose"/>
    /// </summary>
    public void Dispose()
    {
        rSA?.Dispose();
    }

    /// <summary>
    /// Creates an instance of <see cref="ISignatureConfiguration"/> for the an instance of RSA using the parameters in <paramref name="signatureConfiguration"/>
    /// </summary>
    /// <param name="signatureConfiguration"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ISignatureConfiguration CreateConfiguration(ISignatureConfiguration signatureConfiguration)
    {
        using var rsa = RSA.Create();
        
        var publicKeyBytes = rSA!.ExportRSAPublicKey();
        
        var privateKeyBytes = rSA.ExportEncryptedPkcs8PrivateKey(signatureConfiguration.PrivateKeyPassword, new PbeParameters(signatureConfiguration.EncryptionAlgorithm, signatureConfiguration.HashAlgorithmName, signatureConfiguration.IterationCount));
        
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

        if(usePublicKey)
        {
            return GetEncodingOrDefault(signatureConfiguration).GetString(rSA!.DecryptValue(encodedData));
        }

        return GetEncodingOrDefault(signatureConfiguration).GetString(rSA!.Decrypt(encodedData, signatureConfiguration.EncryptionPadding ?? RSAEncryptionPadding.Pkcs1));
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
        
        if (usePublicKey)
        {
            return Convert.ToBase64String(rSA!.EncryptValue(encodedData));
        }

        return Convert.ToBase64String(rSA!.Encrypt(encodedData, signatureConfiguration.EncryptionPadding ?? RSAEncryptionPadding.Pkcs1));
    }
}
