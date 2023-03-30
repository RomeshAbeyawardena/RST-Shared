using RST.Attributes;
using RST.Defaults;
namespace RST.Contracts;

/// <summary>
/// Represents a hashable entitty
/// </summary>
public interface IHashable
{
    /// <summary>
    /// Gets or sets the hash
    /// </summary>
    [HashColumn(nameof(MessagePackModelHasher))]
    string? Hash { get; set; }
}
