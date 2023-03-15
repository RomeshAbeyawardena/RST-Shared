using System.Security.Cryptography;
using System.Text;

namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface ISignatureConfiguration
{
    /// <summary>
    /// 
    /// </summary>
    string? PrivateKey { get; set; }
    /// <summary>
    /// 
    /// </summary>
    string? PublicKey { get; set; } 
    /// <summary>
    /// 
    /// </summary>
    string? PrivateKeyPassword { get; set; }
    /// <summary>
    /// 
    /// </summary>
    HashAlgorithmName HashAlgorithmName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    PbeEncryptionAlgorithm EncryptionAlgorithm { get; set; }
    /// <summary>
    /// 
    /// </summary>
    RSASignaturePadding Padding { get; set; }

    /// <summary>
    /// 
    /// </summary>
    RSAEncryptionPadding EncryptionPadding { get; set; }

    /// <summary>
    /// 
    /// </summary>
    int IterationCount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    Encoding? Encoding { get; set; }
}
