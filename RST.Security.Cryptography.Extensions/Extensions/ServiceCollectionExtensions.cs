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
    /// <param name="services"></param>
    /// <param name="encryptionCaseConvention"></param>
    /// <param name="configureEncryptionOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddEncyptionModuleOption(this IServiceCollection services,
        EncryptionCaseConvention encryptionCaseConvention,
        Action<IDictionaryBuilder<string, IEncryptionOptions>>? configureEncryptionOptions = null)
    {
        return services.AddSingleton<IEncryptionModuleOptions, DefaultEncryptionModuleOptions>(
            (services) =>
        {
            var options = new DefaultEncryptionModuleOptions(encryptionCaseConvention);
            configureEncryptionOptions?.Invoke(new DefaultDictionaryBuilder<string, IEncryptionOptions>(options.EncryptionOptions));
            return options;
        });
    }
}
