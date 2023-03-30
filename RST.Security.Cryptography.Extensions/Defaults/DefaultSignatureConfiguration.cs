using RST.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace RST.Security.Cryptography.Defaults;

/// <summary>
/// 
/// </summary>
public record DefaultSignatureConfiguration : ISignatureConfiguration
{
    /// <summary>
    /// Builds a default configuration from a public key, optional private key and password
    /// </summary>
    /// <param name="publicKey"></param>
    /// <param name="privateKey"></param>
    /// <param name="privateKeyPassword"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static ISignatureConfiguration DefaultConfiguration(string? publicKey, string? privateKey = null, string? privateKeyPassword = null,
        ISignatureConfiguration? configuration = null)
    {
        return new DefaultSignatureConfiguration
        {
            PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey)),
            PrivateKey = privateKey,
            PrivateKeyPassword = privateKeyPassword,
            HashAlgorithmName = configuration?.HashAlgorithmName ?? HashAlgorithmName.SHA256,
            Encoding = configuration?.Encoding ?? Encoding.UTF8,
            EncryptionAlgorithm = configuration?.EncryptionAlgorithm ?? PbeEncryptionAlgorithm.Aes256Cbc,
            EncryptionPadding = configuration?.EncryptionPadding ?? RSAEncryptionPadding.OaepSHA256,
            IterationCount = configuration?.IterationCount ?? 30,
            Padding = configuration?.Padding ?? RSASignaturePadding.Pkcs1,
        };
    }
    /// <summary>
    /// Gets or sets the private key
    /// </summary>
    public string? PrivateKey { get; set; }
    /// <summary>
    /// Gets or sets the public key
    /// </summary>
    public string? PublicKey { get; set; }
    /// <summary>
    /// Gets or sets the private key password
    /// </summary>
    public string? PrivateKeyPassword { get; set; }
    /// <summary>
    /// Gets or sets the hash algorithm name
    /// </summary>
    public HashAlgorithmName HashAlgorithmName { get; set; }
    /// <summary>
    /// Gets or sets the encryption algorithm
    /// </summary>
    public PbeEncryptionAlgorithm EncryptionAlgorithm { get; set; }
    /// <summary>
    /// Gets or sets the padding
    /// </summary>
    public RSASignaturePadding? Padding { get; set; }
    /// <summary>
    /// Gets or sets the iteration count
    /// </summary>
    public int IterationCount { get; set; }
    /// <summary>
    /// Gets or sets the encoding
    /// </summary>
    public Encoding? Encoding { get; set; }
    /// <summary>
    /// Gets or sets the encryption padding
    /// </summary>
    public RSAEncryptionPadding? EncryptionPadding { get; set; }
}
