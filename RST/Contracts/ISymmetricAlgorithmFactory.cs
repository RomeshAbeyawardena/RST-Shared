using System.Security.Cryptography;

namespace RST.Contracts;

/// <summary>
/// Represents a symmetric algorithm factory
/// </summary>
public interface ISymmetricAlgorithmFactory
{
    /// <summary>
    /// Gets the symmetric algorithm used to encrypt and decrypt a stream
    /// </summary>
    /// <param name="symmetricAlgorithm"></param>
    /// <returns></returns>
    SymmetricAlgorithm GetSymmetricAlgorithm(Enumerations.SymmetricAlgorithm symmetricAlgorithm);
}