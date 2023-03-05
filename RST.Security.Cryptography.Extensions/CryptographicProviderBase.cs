using RST.Contracts;
using RST.Enumerations;
using System;
using System.Security.Cryptography;

namespace RST.Security.Cryptography.Extensions;

/// <summary>
/// Presents an abstract cryptographic provider
/// </summary>
public abstract class CryptographicProviderBase
{
    private readonly ISymmetricAlgorithmFactory SymmetricAlgorithmFactory;

    /// <summary>
    /// Gets an instance of <see cref="IEncryptionOptions"/> for supplied <paramref name="encryptionKey"/>
    /// </summary>
    /// <param name="encryptionModuleOptions">Instance of <see cref="IEncryptionModuleOptions"/></param>
    /// <param name="serviceProvider">Instance of service provider</param>
    /// <param name="encryptionKey">Encryption key to retrieve</param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    protected static IEncryptionOptions GetEncryptionOptions(
        IEncryptionModuleOptions encryptionModuleOptions,
        IServiceProvider serviceProvider,
        string encryptionKey)
    {
        if (encryptionModuleOptions.EncryptionOptions.TryGetValue(encryptionKey, out var encryptionOptions))
        {
            return encryptionOptions;
        }
        else if (encryptionModuleOptions.EncryptionOptionsFactory.TryGetValue(encryptionKey, out var encryptionOptionsFactory))
        {
            return encryptionOptionsFactory(serviceProvider);
        }
        else
            throw new NullReferenceException($"Encryption key '{encryptionKey}' not found");
    }

    /// <summary>
    /// Gets a symmetric algorithm 
    /// </summary>
    /// <param name="symmetricAlgorithm"></param>
    /// <returns></returns>
    protected System.Security.Cryptography.SymmetricAlgorithm GetSymmetricAlgorithm(Enumerations.SymmetricAlgorithm symmetricAlgorithm)
    {
        return SymmetricAlgorithmFactory.GetSymmetricAlgorithm(symmetricAlgorithm);
    }

    /// <summary>
    /// Performs case convention operation 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="encryptionCaseConvention"></param>
    /// <returns></returns>
    protected static string PerformEncryptionCaseConventionOperation(
        string value,
        EncryptionCaseConvention encryptionCaseConvention)
    {
        switch (encryptionCaseConvention)
        {
            default:
            case EncryptionCaseConvention.Default:
            case EncryptionCaseConvention.Uppercase:
                return value.ToUpperInvariant();
            case EncryptionCaseConvention.Lowercase:
                return value.ToLowerInvariant();
        }
    }

    /// <summary>
    /// Performs a symmetric operation against a stream
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="encryptionMode"></param>
    /// <param name="encryptionOptions"></param>
    /// <param name="cryptoStreamMode"></param>
    /// <param name="operation"></param>
    /// <param name="memoryStreamBuffer"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    protected T ExecuteSymmetricOperation<T>(
        EncryptionMode encryptionMode,
        IEncryptionOptions encryptionOptions,
        CryptoStreamMode cryptoStreamMode,
        Func<MemoryStream, CryptoStream, T> operation,
        byte[]? memoryStreamBuffer = null)
    {
        ICryptoTransform GetCryptoTransform(System.Security.Cryptography.SymmetricAlgorithm symmetricAlgorithm)
        {
            if (string.IsNullOrWhiteSpace(encryptionOptions.Key))
            {
                throw new NullReferenceException("Key must not be null or an empty value");
            }

            if (string.IsNullOrWhiteSpace(encryptionOptions.InitialVector))
            {
                throw new NullReferenceException("Initial vector must not be null or an empty value");
            }

            var key = Convert.FromBase64String(encryptionOptions.Key);
            var iv = Convert.FromBase64String(encryptionOptions.InitialVector);

            if (key.Length > 32)
                throw new InvalidOperationException("Key is an invalid size");

            if (iv.Length > 16 || iv.Length < 16)
                throw new InvalidOperationException("Initial vector an invalid size");

            switch (encryptionMode)
            {

                case EncryptionMode.Encrypt:
                    return symmetricAlgorithm.CreateEncryptor(key, iv);
                case EncryptionMode.Decrypt:
                    return symmetricAlgorithm.CreateDecryptor(key, iv);
                default:
                    throw new NotSupportedException();
            }
        }

        using (var memoryStream = memoryStreamBuffer == null
            ? new MemoryStream()
            : new MemoryStream(memoryStreamBuffer))
        {
            using (var algorithm = GetSymmetricAlgorithm(encryptionOptions.Algorithm.GetValueOrDefault()))
            using (var encryptor = GetCryptoTransform(algorithm))
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, cryptoStreamMode))
                return operation(memoryStream, cryptoStream);
        }
    }

    /// <summary>
    /// When inherited in a class implements an instance of <see cref="CryptographicProviderBase"/>
    /// </summary>
    /// <param name="encryptionOptions"></param>
    /// <param name="symmetricAlgorithmFactory"></param>
    public CryptographicProviderBase(IEncryptionOptions encryptionOptions, ISymmetricAlgorithmFactory symmetricAlgorithmFactory)
    {
        EncryptionOptions = encryptionOptions;
        SymmetricAlgorithmFactory = symmetricAlgorithmFactory;
    }

    /// <summary>
    /// Gets a default instance of encrpytion options 
    /// </summary>
    public IEncryptionOptions EncryptionOptions { get; }
}
