using RST.Contracts;
using System.Security.Cryptography;

namespace RST.Security.Cryptography.Extensions.Defaults;

/// <inheritdoc cref="IEncryptor"/>
public class DefaultEncryptor : IEncryptor
{
    ///<inheritdoc cref="IEncryptor.Encrypt(string, IEncryptionOptions)"/>
    public string Encrypt(string value, IEncryptionOptions encryptionOptions)
    {
        MemoryStream EncryptOperation(MemoryStream memoryStream, CryptoStream cryptoStream)
        {
            using (var streamWriter = new StreamWriter(cryptoStream, encryptionOptions.Encoding))
                streamWriter.Write(value);
            return memoryStream;
        }

        value = PerformEncryptionCaseConventionOperation(value, encryptionModuleOptions.EncryptionCaseConvention);

        var encryptedBytes = ExecuteSymmetricOperation(Shared.Enumerations.EncryptionMode.Encrypt,
            encryptionOptions, CryptoStreamMode.Write, EncryptOperation).ToArray();


        return Convert.ToBase64String(encryptedBytes);
    }
}
