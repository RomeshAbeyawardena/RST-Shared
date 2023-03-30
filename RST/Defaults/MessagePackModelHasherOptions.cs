using System.Security.Cryptography;

namespace RST.Defaults;

/// <summary>
/// 
/// </summary>
public record MessagePackModelHasherOptions
{
    /// <summary>
    /// 
    /// </summary>
    public HashAlgorithmName HashAlgorithmName { get; set; }
}
