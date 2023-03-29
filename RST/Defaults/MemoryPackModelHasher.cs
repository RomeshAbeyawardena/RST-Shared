using MessagePack;
using RST.Attributes;
using RST.Contracts;

namespace RST.Defaults;

/// <summary>
/// 
/// </summary>
[Register]
public class MemoryPackModelHasher : ModelHasherBase<MemoryPackModelHasherOptions>
{
    private readonly IHashAlgorithmProvider hashAlgorithmProvider;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hashAlgorithmProvider"></param>
    public MemoryPackModelHasher(IHashAlgorithmProvider hashAlgorithmProvider)
        : base(nameof(MemoryPackModelHasher))
    {
        this.hashAlgorithmProvider = hashAlgorithmProvider;
    }

    /// <inheritdoc cref="IModelHasher{TModelHasherOptions}.CalculateHash{T}(T, TModelHasherOptions)"/>
    public override string CalculateHash<T>(T model, MemoryPackModelHasherOptions options)
    {
        using var hashAlgorithm = hashAlgorithmProvider.GetHashAlgorithm(options.HashAlgorithmName); 
        var serialisedModel = MessagePackSerializer.Serialize(model);
        var secureHash = hashAlgorithm.ComputeHash(serialisedModel);
        return Convert.ToBase64String(secureHash);
    }
}
