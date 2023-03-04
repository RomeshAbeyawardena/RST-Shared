namespace RST.Enumerations;

/// <summary>
/// Represents the encryption case convention
/// </summary>
public enum EncryptionCaseConvention
{
    /// <summary>
    /// Uses the original casing used by the input
    /// </summary>
    Default,
    /// <summary>
    /// Transforms the input to upper case
    /// </summary>
    Uppercase,
    /// <summary>
    /// Transforms the input to lower case
    /// </summary>
    Lowercase
}
