using RST.Attributes;
using RST.Contracts;
using System.Security.Cryptography;

namespace RST.Security.Cryptography.Extensions.Defaults;


/// <inheritdoc cref="ISymmetricAlgorithmFactory"/>
[Register]
public class DefaultSymmetricAlgorithmFactory : ISymmetricAlgorithmFactory
{
    IReadOnlyDictionary<Enumerations.SymmetricAlgorithm, Func<SymmetricAlgorithm>> symmetricAlgorithms;
    /// <summary>
    /// Initialises an instance of <see cref="DefaultSymmetricAlgorithmFactory"/>
    /// </summary>
    public DefaultSymmetricAlgorithmFactory()
    {
        symmetricAlgorithms = new Dictionary<Enumerations.SymmetricAlgorithm, Func<SymmetricAlgorithm>>() { 
            { Enumerations.SymmetricAlgorithm.Aes, Aes.Create }, 
            { Enumerations.SymmetricAlgorithm.TripleDES, TripleDES.Create } 
        };
    }

    /// <inheritdoc cref="ISymmetricAlgorithmFactory.GetSymmetricAlgorithm(Enumerations.SymmetricAlgorithm)"/>
    /// <exception cref="NullReferenceException">When algorithm is not found</exception>
    public SymmetricAlgorithm GetSymmetricAlgorithm(Enumerations.SymmetricAlgorithm symmetricAlgorithm)
    {
        if(symmetricAlgorithms.TryGetValue(symmetricAlgorithm, out var getAlgorithm))
        {
            return getAlgorithm();
        }

        throw new NullReferenceException("Algorithm not found!");
    }
}
