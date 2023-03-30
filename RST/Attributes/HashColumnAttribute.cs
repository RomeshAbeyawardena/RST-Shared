using System.Security.Cryptography;

namespace RST.Attributes;

/// <summary>
/// Represents a hash column
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class HashColumnAttribute : MessagePack.IgnoreMemberAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="hasherImplementation"></param>
    public HashColumnAttribute(string name, string? hasherImplementation = null)
    {
        Name = new HashAlgorithmName(name);
        HasherImplementation = hasherImplementation;
    }

    /// <summary>
    /// Gets the hash algorithm name
    /// </summary>
    public HashAlgorithmName Name { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? HasherImplementation { get; }
}
