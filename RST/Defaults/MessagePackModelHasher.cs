using MessagePack;
using RST.Attributes;
using RST.Contracts;

namespace RST.Defaults;

/// <summary>
/// 
/// </summary>
[Register]
public class MessagePackModelHasher : ModelHasherBase<MessagePackModelHasherOptions>
{
    private readonly IHashAlgorithmProvider hashAlgorithmProvider;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hashAlgorithmProvider"></param>
    public MessagePackModelHasher(IHashAlgorithmProvider hashAlgorithmProvider)
        : base(nameof(MessagePackModelHasher), true)
    {
        this.hashAlgorithmProvider = hashAlgorithmProvider;
    }

    /// <inheritdoc cref="IModelHasher{TModelHasherOptions}.CalculateHash{T}(T, TModelHasherOptions)"/>
    public override string CalculateHash<T>(T model, MessagePackModelHasherOptions options)
    {
        options = options ?? DefaultOptions ?? MessagePackModelHasherOptions.DefaultOptions;
        using var hashAlgorithm = hashAlgorithmProvider.GetHashAlgorithm(options.HashAlgorithmName); 
        var serialisedModel = MessagePackSerializer.Serialize(model);
        var secureHash = hashAlgorithm.ComputeHash(serialisedModel);
        return Convert.ToBase64String(secureHash);
    }
}
