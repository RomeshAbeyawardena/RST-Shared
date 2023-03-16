using RST.Contracts;
using RST.Enumerations;

namespace RST.Security.Cryptography.Extensions.Defaults;

/// <inheritdoc cref="IEncryptionModuleOptions"/>
public record DefaultEncryptionModuleOptions : IEncryptionModuleOptions
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initialises a new instance of <see cref="IEncryptionModuleOptions"/>
    /// </summary>
    /// <param name="encryptionCaseConvention"></param>
    /// <param name="serviceProvider"></param>
    public DefaultEncryptionModuleOptions(EncryptionCaseConvention encryptionCaseConvention,
        IServiceProvider serviceProvider)
    {
        EncryptionOptionsFactory = new Dictionary<string, Func<IServiceProvider, IEncryptionOptions>>();

        EncryptionOptions = new Dictionary<string, IEncryptionOptions>();
        EncryptionCaseConvention = encryptionCaseConvention;
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IEncryptionOptions? this[string key] { get =>
            EncryptionOptions.TryGetValue(key, out var options)
            ? options : EncryptionOptionsFactory.TryGetValue(key, out var factory)
                ? factory(serviceProvider) : null;
    }

    /// <inheritdoc cref="IEncryptionModuleOptions.EncryptionOptionsFactory"/>
    public IDictionary<string, Func<IServiceProvider, IEncryptionOptions>> EncryptionOptionsFactory { get; }
    /// <inheritdoc cref="IEncryptionModuleOptions.EncryptionOptions"/>
    public IDictionary<string, IEncryptionOptions> EncryptionOptions { get; }
    /// <inheritdoc cref="IEncryptionModuleOptions.EncryptionCaseConvention"/>
    public EncryptionCaseConvention EncryptionCaseConvention { get; set; }
}
