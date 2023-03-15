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
    /// 
    /// </summary>
    public string? PrivateKey { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? PublicKey { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? PrivateKeyPassword { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public HashAlgorithmName HashAlgorithmName { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public PbeEncryptionAlgorithm EncryptionAlgorithm { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public RSASignaturePadding Padding { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int IterationCount { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Encoding? Encoding { get; set; }
}
