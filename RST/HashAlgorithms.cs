using System.Security.Cryptography;

namespace RST;

/// <summary>
/// Gets a list of Algorithms
/// </summary>
public static class HashAlgorithms
{
    /// <summary>
    /// Gets a <see cref="HashAlgorithmName" /> representing "MD5"
    /// </summary>
    public const string MD5 = "MD5";

    /// <summary>
    /// Gets a string representing "SHA1"
    /// </summary>
    public const string SHA1 = "SHA1";

    /// <summary>
    /// Gets a string representing "SHA256"
    /// </summary>
    public const string SHA256 = "SHA256";

    /// <summary>
    /// Gets a string representing "SHA384"
    /// </summary>
    public const string SHA384 = "SHA384";

    /// <summary>
    /// Gets a string representing "SHA512"
    /// </summary>
    public const string SHA512 = "SHA512";

}
