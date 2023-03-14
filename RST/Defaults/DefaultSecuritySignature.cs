using RST.Attributes;
using RST.Contracts;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace RST.Defaults;

/// <summary>
/// 
/// </summary>
[Register(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
public class DefaultSecuritySignature : ISecuritySignature
{
    private RSA rSA;

    internal void FlushProvider()
    {
        rSA.Dispose();
        rSA = RSA.Create();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rSA"></param>
    public DefaultSecuritySignature(RSA rSA)
    {
        this.rSA = rSA;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="signature"></param>
    /// <param name="signatureConfiguration"></param>
    /// <returns></returns>
    public bool VerifyData(string data, string signature, ISignatureConfiguration signatureConfiguration)
    {
        var publicKeyBytes = Convert.FromBase64String(signatureConfiguration.PublicKey ?? throw new ArgumentException());
        rSA.ImportRSAPublicKey(publicKeyBytes, out var bytes);
        
        return rSA.VerifyData((signatureConfiguration.Encoding ?? Encoding.Default).GetBytes(data), 
            Convert.FromBase64String(signature), signatureConfiguration.HashAlgorithmName, signatureConfiguration.Padding);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="signatureConfiguration"></param>
    /// <returns></returns>
    public string SignData(string data, ISignatureConfiguration signatureConfiguration)
    {
        byte[] privateKeyBytes,
               publicKeyBytes;

        if (string.IsNullOrEmpty(signatureConfiguration.PublicKey))
        {
            publicKeyBytes = rSA.ExportRSAPublicKey();
            Debug.WriteLine(Convert.ToBase64String(publicKeyBytes), "Public key");
        }
        else
        {
            publicKeyBytes = Convert.FromBase64String(signatureConfiguration.PublicKey);
            rSA.ImportRSAPublicKey(publicKeyBytes, out var bytes);
        }

        if (string.IsNullOrEmpty(signatureConfiguration.PrivateKey))
        {
            privateKeyBytes = rSA.ExportEncryptedPkcs8PrivateKey(signatureConfiguration.PrivateKeyPassword, new PbeParameters(signatureConfiguration.EncryptionAlgorithm, signatureConfiguration.HashAlgorithmName, signatureConfiguration.IterationCount));

            Debug.WriteLine(Convert.ToBase64String(privateKeyBytes), "Private key");
        }
        else
        {
            privateKeyBytes = Convert.FromBase64String(signatureConfiguration.PrivateKey);
            rSA.ImportEncryptedPkcs8PrivateKey(signatureConfiguration.PrivateKeyPassword, privateKeyBytes, out var bytes);
        }

        var signature = rSA.SignData(Encoding.UTF8.GetBytes(data), signatureConfiguration.HashAlgorithmName, signatureConfiguration.Padding);

        //Debug.WriteLine(Convert.ToBase64String(signature), "Signature");
        return Convert.ToBase64String(signature);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        rSA.Dispose();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="signatureConfiguration"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ISignatureConfiguration CreateConfiguration(ISignatureConfiguration signatureConfiguration)
    {
        using var rsa = RSA.Create();
        var configuration = new DefaultSignatureConfiguration();
        var publicKeyBytes = rSA.ExportRSAPublicKey();
        configuration.PublicKey = Convert.ToBase64String(publicKeyBytes);
        var privateKeyBytes = rSA.ExportEncryptedPkcs8PrivateKey(signatureConfiguration.PrivateKeyPassword, new PbeParameters(signatureConfiguration.EncryptionAlgorithm, signatureConfiguration.HashAlgorithmName, signatureConfiguration.IterationCount));
        configuration.PrivateKey = Convert.ToBase64String(privateKeyBytes);

        return configuration;
    }
}
