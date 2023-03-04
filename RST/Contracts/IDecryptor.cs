namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface IDecryptor
{
    /// <summary>
    /// Decrypts a <paramref name="value"/>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    string Decrypt(string value, IEncryptionOptions options);
}
