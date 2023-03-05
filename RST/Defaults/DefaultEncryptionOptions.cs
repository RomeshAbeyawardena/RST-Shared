using RST.Contracts;
using RST.Enumerations;
using System.Text;

namespace RST.Defaults;

/// <inheritdoc cref="IEncryptionOptions"/>
public record DefaultEncryptionOptions : IEncryptionOptions
{
    /// <inheritdoc cref="IEncryptionOptions.Algorithm"/>
    public SymmetricAlgorithm? Algorithm { get; set; }

    /// <inheritdoc cref="IEncryptionOptions.Key"/>
    public string? Key { get; set; }

    /// <inheritdoc cref="IEncryptionOptions.InitialVector"/>
    public string? InitialVector { get; set; }

    /// <inheritdoc cref="IEncryptionOptions.Encoding"/>
    public Encoding? Encoding { get; set; }
}
