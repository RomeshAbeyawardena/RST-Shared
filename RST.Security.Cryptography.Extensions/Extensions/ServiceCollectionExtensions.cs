using Microsoft.Extensions.DependencyInjection;
using RST.Contracts;
using RST.Defaults;
using RST.Enumerations;
using RST.Security.Cryptography.Extensions.Defaults;

namespace RST.Security.Cryptography.Extensions;
/// <summary>
/// Represents service collection extensions
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the encryption module option to the provided <paramref name="services"/> parameter
    /// </summary>
    /// <param name="services">Instance of <see cref="IServiceCollection"/> to register the encryption module options to</param>
    /// <param name="encryptionCaseConvention">Default encryption case convention</param>
    /// <param name="configureEncryptionOptions">Configures encryption dictionary</param>
    /// <param name="configureServiceEncryptionOptions">Configures encryption factory dictionary</param>
    /// <returns></returns>
    public static IServiceCollection AddEncyptionModuleOption(this IServiceCollection services,
        EncryptionCaseConvention encryptionCaseConvention = EncryptionCaseConvention.Default,
        Action<IDictionaryBuilder<string, IEncryptionOptions>>? configureEncryptionOptions = null,
        Action<IDictionaryBuilder<string, Func<IServiceProvider, IEncryptionOptions>>>? configureServiceEncryptionOptions = null)
    {
        return services.AddSingleton<IEncryptionModuleOptions, DefaultEncryptionModuleOptions>(
            (services) =>
        {
            var options = new DefaultEncryptionModuleOptions(encryptionCaseConvention);
            configureEncryptionOptions?.Invoke(new DefaultDictionaryBuilder<string, IEncryptionOptions>(options.EncryptionOptions));
            configureServiceEncryptionOptions?.Invoke(new DefaultDictionaryBuilder<string, Func<IServiceProvider, IEncryptionOptions>>(options.EncryptionOptionsFactory));
            return options;
        });
    }
}
