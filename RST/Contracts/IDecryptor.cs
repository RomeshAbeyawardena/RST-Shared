namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface IDecryptor
{
    /// <summary>
    /// Decrypts a <paramref name="value"/>
    /// </summary>
    /// <param name="value">Value to encrypt</param>
    /// <param name="options">Options to apply Decryption</param>
    /// <returns>Decrypted value</returns>
    string Decrypt(string value, IEncryptionOptions options);

    /// <summary>
    /// Decrypt a <paramref name="value"/> using the supplied encryption key registered in the service provider
    /// </summary>
    /// <param name="encryptionKey">Key to obtain an instance of <see cref="IEncryptionOptions"/> to apply encryption</param>
    /// <param name="value">Value to encrypt</param>
    /// <returns>Decrypted value</returns>
    string Decrypt(string encryptionKey, string value);
}
