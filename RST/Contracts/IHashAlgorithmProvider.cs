using System.Security.Cryptography;

namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface IHashAlgorithmProvider
{
    /// <summary>
    /// Get an instance of hash algorithm by the parameter of <paramref name="hashAlgorithmName"/>
    /// </summary>
    /// <returns></returns>
    HashAlgorithm? GetHashAlgorithm(HashAlgorithmName hashAlgorithmName);
}
