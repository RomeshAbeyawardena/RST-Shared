using RST.Attributes;
using RST.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace RST.Security.Cryptography.Extensions.Defaults;

/// <inheritdoc cref="IDecryptor"/>
[Register]
public class DefaultDecryptor : CryptographicProviderBase, IDecryptor
{
    private readonly IEncryptionModuleOptions encryptionModuleOptions;
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initialises an instance of <see cref="DefaultDecryptor"/>
    /// </summary>
    /// <param name="encryptionOptions"></param>
    /// <param name="symmetricAlgorithmFactory"></param>
    /// <param name="encryptionModuleOptions"></param>
    /// <param name="serviceProvider"></param>
    public DefaultDecryptor(IEncryptionOptions encryptionOptions, ISymmetricAlgorithmFactory symmetricAlgorithmFactory, IEncryptionModuleOptions encryptionModuleOptions, IServiceProvider serviceProvider) : base(encryptionOptions, symmetricAlgorithmFactory)
    {
        this.encryptionModuleOptions = encryptionModuleOptions;
        this.serviceProvider = serviceProvider;
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

    /// <inheritdoc cref="IDecryptor.Decrypt(string, string)"/>
    public string Decrypt(string encryptionKey, string value)
    {
        var encryptionOptions = GetEncryptionOptions(encryptionModuleOptions, serviceProvider, encryptionKey);

        return Decrypt(value, encryptionOptions);
    }
}
