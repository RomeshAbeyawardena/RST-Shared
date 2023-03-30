using RST.Attributes;
using RST.Contracts;
using System.Security.Cryptography;

namespace RST.Defaults;

/// <summary>
/// 
/// </summary>
[Register]
public class DefaultHashAlgorithmProvider : IHashAlgorithmProvider
{
    private IReadOnlyDictionary<HashAlgorithmName, Func<HashAlgorithm>> hashAlgorithmFactory =>
        new Dictionary<HashAlgorithmName, Func<HashAlgorithm>>()
        {
            { HashAlgorithmName.MD5, () => MD5.Create() },
            { HashAlgorithmName.SHA1, () => SHA1.Create() },
            { HashAlgorithmName.SHA256, () => SHA256.Create() },
            { HashAlgorithmName.SHA384, () => SHA384.Create() },
            { HashAlgorithmName.SHA512, () => SHA512.Create() }
        };
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hashAlgorithmName"></param>
    /// <returns></returns>
    public HashAlgorithm? GetHashAlgorithm(HashAlgorithmName hashAlgorithmName)
    {
        return hashAlgorithmFactory.TryGetValue(hashAlgorithmName, out var factory) 
            ? factory() : null;
    }
}
