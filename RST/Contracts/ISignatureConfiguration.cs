using System.Security.Cryptography;
using System.Text;

namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface ISignatureConfiguration
{
    /// <summary>
    /// Gets or sets the private key
    /// </summary>
    string? PrivateKey { get; set; }
    /// <summary>
    /// Gets or sets the public key
    /// </summary>
    string? PublicKey { get; set; }
    /// <summary>
    /// Gets or sets the private key password
    /// </summary>
    string? PrivateKeyPassword { get; set; }
    /// <summary>
    /// Gets or sets the hash algorithm name
    /// </summary>
    HashAlgorithmName HashAlgorithmName { get; set; }
    /// <summary>
    /// Gets or sets the encryption algorithm
    /// </summary>
    PbeEncryptionAlgorithm EncryptionAlgorithm { get; set; }
    /// <summary>
    /// Gets or sets the signature padding
    /// </summary>
    RSASignaturePadding? Padding { get; set; }

    /// <summary>
    /// Gets or sets the encryption padding
    /// </summary>
    RSAEncryptionPadding? EncryptionPadding { get; set; }

    /// <summary>
    /// Gets or sets the iteration count
    /// </summary>
    int IterationCount { get; set; }
    /// <summary>
    /// Gets or sets the encoding
    /// </summary>
    Encoding? Encoding { get; set; }
}
