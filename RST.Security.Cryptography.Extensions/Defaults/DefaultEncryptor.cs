using RST.Attributes;
using RST.Contracts;
using System.Security.Cryptography;
using System.Text;

namespace RST.Security.Cryptography.Extensions.Defaults;

/// <inheritdoc cref="IEncryptor"/>
[Register]
public class DefaultEncryptor : CryptographicProviderBase, IEncryptor
{
    private readonly IEncryptionModuleOptions encryptionModuleOptions;

    /// <summary>
    /// Initialises instance of <see cref="IEncryptor"/> implementation
    /// </summary>
    /// <param name="encryptionOptions"></param>
    /// <param name="symmetricAlgorithmFactory"></param>
    /// <param name="encryptionModuleOptions"></param>
    public DefaultEncryptor(IEncryptionOptions encryptionOptions, 
        ISymmetricAlgorithmFactory symmetricAlgorithmFactory,
        IEncryptionModuleOptions encryptionModuleOptions) : base(encryptionOptions, symmetricAlgorithmFactory)
    {
        this.encryptionModuleOptions = encryptionModuleOptions;
    }

    ///<inheritdoc cref="IEncryptor.Encrypt(string, IEncryptionOptions)"/>
    public string Encrypt(string value, IEncryptionOptions encryptionOptions)
    {
        MemoryStream EncryptOperation(MemoryStream memoryStream, CryptoStream cryptoStream)
        {
            using (var streamWriter = new StreamWriter(cryptoStream, encryptionOptions.Encoding
                ?? Encoding.Default))
                streamWriter.Write(value);
            return memoryStream;
        }

        value = PerformEncryptionCaseConventionOperation(value, encryptionModuleOptions.EncryptionCaseConvention);

        var encryptedBytes = ExecuteSymmetricOperation(Enumerations.EncryptionMode.Encrypt,
            encryptionOptions, CryptoStreamMode.Write, EncryptOperation).ToArray();

        return Convert.ToBase64String(encryptedBytes);
    }
}
