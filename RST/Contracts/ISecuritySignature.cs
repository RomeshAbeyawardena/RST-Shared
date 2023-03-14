namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface ISecuritySignature : IDisposable
{
    /// <summary>
    /// Generates a signature for <paramref name="data"/>
    /// </summary>
    /// <param name="data"></param>
    /// <param name="signatureConfiguration"></param>
    /// <returns></returns>
    string SignData(string data, ISignatureConfiguration signatureConfiguration);
    /// <summary>
    /// Verifies signed <paramref name="data"/>
    /// </summary>
    /// <param name="data"></param>
    /// <param name="signature"></param>
    /// <param name="signatureConfiguration"></param>
    /// <returns></returns>
    bool VerifyData(string data, string signature, ISignatureConfiguration signatureConfiguration);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="signatureConfiguration"></param>
    /// <returns></returns>
    ISignatureConfiguration CreateConfiguration(ISignatureConfiguration signatureConfiguration);
}
