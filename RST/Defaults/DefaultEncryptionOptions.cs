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

    /// <summary>
    /// Creates a new instance of <see cref="DefaultEncryptionOptions"/> with shared property values
    /// </summary>
    /// <param name="initialVector"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEncryptionOptions CreateInstance(string initialVector, string? key = null)
    {
        return new DefaultEncryptionOptions() {
            Algorithm = Algorithm,
            Key = string.IsNullOrEmpty(key) ? Key : key,
            InitialVector = initialVector,
            Encoding = Encoding,
        };
    }
}
