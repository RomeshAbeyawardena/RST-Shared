using RST.Enumerations;

namespace RST.Contracts;

/// <summary>
/// Represents encryption module options
/// </summary>
public interface IEncryptionModuleOptions
{
    /// <summary>
    /// Gets a named instance factory of <see cref="IEncryptionOptions"/>
    /// </summary>
    IDictionary<string, Func<IServiceProvider, IEncryptionOptions>> EncryptionOptionsFactory { get; }
    /// <summary>
    /// Gets a named instance factory of <see cref="IEncryptionOptions"/>
    /// </summary>
    IDictionary<string, IEncryptionOptions> EncryptionOptions { get; }

    /// <summary>
    /// Gets or sets the default encryption case conventions to use
    /// </summary>
    EncryptionCaseConvention EncryptionCaseConvention { get; set; }
}
