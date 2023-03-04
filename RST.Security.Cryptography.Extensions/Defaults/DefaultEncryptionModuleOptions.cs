using RST.Contracts;
using RST.Enumerations;

namespace RST.Security.Cryptography.Extensions.Defaults;

/// <inheritdoc cref="IEncryptionModuleOptions"/>
public record DefaultEncryptionModuleOptions : IEncryptionModuleOptions
{
    /// <summary>
    /// Initialises a new instance of <see cref="IEncryptionModuleOptions"/>
    /// </summary>
    /// <param name="encryptionCaseConvention"></param>
    public DefaultEncryptionModuleOptions(EncryptionCaseConvention encryptionCaseConvention)
    {
        EncryptionOptionsFactory = new Dictionary<string, Func<IServiceProvider, IEncryptionOptions>>();

        EncryptionOptions = new Dictionary<string, IEncryptionOptions>();
        EncryptionCaseConvention = encryptionCaseConvention;
    }
    /// <inheritdoc cref="IEncryptionModuleOptions.EncryptionOptionsFactory"/>
    public IDictionary<string, Func<IServiceProvider, IEncryptionOptions>> EncryptionOptionsFactory { get; }
    /// <inheritdoc cref="IEncryptionModuleOptions.EncryptionOptions"/>
    public IDictionary<string, IEncryptionOptions> EncryptionOptions { get; }
    /// <inheritdoc cref="IEncryptionModuleOptions.EncryptionCaseConvention"/>
    public EncryptionCaseConvention EncryptionCaseConvention { get; set; }
}
