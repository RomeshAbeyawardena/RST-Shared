using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RST.Contracts;
using RST.Defaults;

namespace RST.Extensions.Configuration;
/// <summary>
/// 
/// </summary>
public static class DictionaryBuilderExtensions
{
    /// <summary>
    /// Adds encryption options from a configuration
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="rootPath"></param>
    /// <param name="sectionNames"></param>
    /// <returns></returns>
    public static IDictionary<string, Func<string, IEncryptionOptions>>
        AddConfiguration(this IDictionary<string, IEncryptionOptions> dictionary, IServiceProvider serviceProvider,
            string rootPath, params string[] sectionNames)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        var options = configuration.GetValues<IEncryptionOptions>(rootPath, sectionNames);

        for (int i = 0; i < sectionNames.Length; i++)
        {
            var sectionName = sectionNames[i];
            var option = options.ElementAt(i);

            if (option != null)
            {
                dictionary.Add(sectionName, s => option);
            }
        }

        return dictionary;
    }
    /// <summary>
    /// Adds encryption options from a configuration key or customised options
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="encryptionConfigurationKey"></param>
    /// <param name="configureOptions"></param>
    /// <exception cref="NullReferenceException"></exception>
    /// <returns></returns>
    public static IDictionaryBuilder<string, Func<IServiceProvider, IEncryptionOptions>>
        AddConfiguration(this IDictionaryBuilder<string, Func<IServiceProvider, IEncryptionOptions>> dictionary, string key, string encryptionConfigurationKey, Func<IConfiguration, IEncryptionOptions>? configureOptions = null)
    {
        dictionary.Add(key, e =>
        {
            var configuration = e.GetRequiredService<IConfiguration>();

            if (configureOptions != null)
            {
                return configureOptions(configuration);
            }

            return configuration
                .GetSection(encryptionConfigurationKey)
                .Get<IEncryptionOptions>() ?? throw new NullReferenceException("Unable to bind configuration"); ;

        });
        return dictionary;
    }
}
