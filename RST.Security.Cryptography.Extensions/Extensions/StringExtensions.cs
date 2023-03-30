using RST.Contracts;

namespace RST.Security.Cryptography.Extensions;

/// <summary>
/// Represents string extensions pertaining to Security.Cryptography;
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// <inheritdoc cref="IEncryptor.Encrypt(string, string)"/> or supplied <paramref name="encryptionOptions"/>
    /// </summary>
    /// <param name="input"></param>
    /// <param name="encryptor"></param>
    /// <param name="encryptionOptions"></param>
    /// <param name="encryptionKey"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static string Encrypt(this string input, IEncryptor encryptor, IEncryptionOptions? encryptionOptions = null,
        string? encryptionKey = null)
    {
        if (encryptor == null)
        {
            throw new ArgumentNullException(nameof(encryptor));
        }

        if (encryptionOptions != null)
        {
            return encryptor.Encrypt(input, encryptionOptions);
        }

        if (encryptionKey != null)
        {
            return encryptor.Encrypt(input, encryptionKey);
        }

        throw new ArgumentException("Must supply either encryptionOptions or encryptionKey as a parameter");
    }

    /// <summary>
    /// <inheritdoc cref="IDecryptor.Decrypt(string, string)"/> or supplied <paramref name="encryptionOptions"/>
    /// </summary>
    /// <param name="input"></param>
    /// <param name="decryptor"></param>
    /// <param name="encryptionOptions"></param>
    /// <param name="encryptionKey"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string Decrypt(this string input, IDecryptor decryptor, IEncryptionOptions? encryptionOptions = null,
        string? encryptionKey = null)
    {
        if (decryptor == null)
        {
            throw new ArgumentNullException(nameof(decryptor));
        }

        if (encryptionOptions != null)
        {
            return decryptor.Decrypt(input, encryptionOptions);
        }

        if (encryptionKey != null)
        {
            return decryptor.Decrypt(input, encryptionKey);
        }

        throw new ArgumentException("Must supply either encryptionOptions or encryptionKey as a parameter");
    }
}
