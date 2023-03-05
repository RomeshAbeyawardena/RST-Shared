namespace RST.Contracts;

/// <summary>
/// Represents an encryptor
/// </summary>
public interface IEncryptor
{
    /// <summary>
    /// Encrypts a <paramref name="value"/> using the supplied <paramref name="options"/>
    /// </summary>
    /// <param name="value">Value to encrypt</param>
    /// <param name="options">Options to apply encryption</param>
    /// <returns>Encrypted value</returns>
    string Encrypt(string value, IEncryptionOptions options);

    /// <summary>
    /// Encrypts a <paramref name="value"/> using the supplied encryption key registered in the service provider
    /// </summary>
    /// <param name="encryptionKey">Key to obtain an instance of <see cref="IEncryptionOptions"/> to apply encryption</param>
    /// <param name="value">Value to encrypt</param>
    /// <returns>Encrypted value</returns>
    string Encrypt(string encryptionKey, string value);
}
