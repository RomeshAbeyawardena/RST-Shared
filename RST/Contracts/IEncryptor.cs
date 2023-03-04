namespace RST.Contracts;

/// <summary>
/// Represents an encryptor
/// </summary>
public interface IEncryptor
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    string Encrypt(string value, IEncryptionOptions options);
}
