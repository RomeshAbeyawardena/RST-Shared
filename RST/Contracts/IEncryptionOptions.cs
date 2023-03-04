using System.Text;

namespace RST.Contracts;

/// <summary>
/// Represents encryption options
/// </summary>
public interface IEncryptionOptions
{
    /// <summary>
    /// Gets or sets the symmetric algorithm
    /// </summary>
    Enumerations.SymmetricAlgorithm? Algorithm { get; set; }
    /// <summary>
    /// Gets or sets the encryption key
    /// </summary>
    string Key { get; set; }
    /// <summary>
    /// Gets or sets the public key
    /// </summary>
    string InitialVector { get; set; }

    Encoding? Encoding { get; set; }
}
