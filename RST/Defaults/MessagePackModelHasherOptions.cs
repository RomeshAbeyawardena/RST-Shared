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
    public static MessagePackModelHasherOptions DefaultOptions => new()
    {
        HashAlgorithmName = HashAlgorithmName.SHA256
    };
    /// <summary>
    /// 
    /// </summary>
    public HashAlgorithmName HashAlgorithmName { get; set; }
}
