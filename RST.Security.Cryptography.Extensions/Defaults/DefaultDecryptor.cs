using RST.Attributes;
using RST.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace RST.Security.Cryptography.Extensions.Defaults;

/// <inheritdoc cref="IDecryptor"/>
[Register]
public class DefaultDecryptor : CryptographicProviderBase, IDecryptor
{
    /// <summary>
    /// Initialises an instance of <see cref="DefaultDecryptor"/>
    /// </summary>
    /// <param name="encryptionOptions"></param>
    /// <param name="symmetricAlgorithmFactory"></param>
    public DefaultDecryptor(IEncryptionOptions encryptionOptions, ISymmetricAlgorithmFactory symmetricAlgorithmFactory) : base(encryptionOptions, symmetricAlgorithmFactory)
    {
    }

    /// <inheritdoc cref="IDecryptor.Decrypt(string, IEncryptionOptions)"/>
    public string Decrypt(string value, IEncryptionOptions encryptionOptions)
    {
        string DecryptOperation(MemoryStream memoryStream, CryptoStream cryptoStream)
        {
            using (var streamReader = new StreamReader(cryptoStream, encryptionOptions.Encoding
                ?? Encoding.Default))
            {
                return streamReader.ReadToEnd();
            }
        }

        var encryptedBytes = Convert.FromBase64String(value);

        return ExecuteSymmetricOperation(Enumerations.EncryptionMode.Decrypt,
            encryptionOptions, CryptoStreamMode.Read, DecryptOperation, encryptedBytes);
    }
}
