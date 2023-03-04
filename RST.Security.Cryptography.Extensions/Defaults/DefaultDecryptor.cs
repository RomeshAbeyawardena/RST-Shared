using RST.Contracts;

namespace RST.Security.Cryptography.Extensions.Defaults;

/// <inheritdoc cref="IDecryptor"/>
public class DefaultDecryptor : IDecryptor
{
    /// <inheritdoc cref="IDecryptor.Decrypt(string, IEncryptionOptions)"/>
    public string Decrypt(string value, IEncryptionOptions options)
    {
        throw new NotImplementedException();
    }
}
