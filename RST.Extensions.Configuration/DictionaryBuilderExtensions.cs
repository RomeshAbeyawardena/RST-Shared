using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RST.Contracts;

namespace RST.Extensions.Configuration;
/// <summary>
/// 
/// </summary>
public static class DictionaryBuilderExtensions
{
    /// <summary>
    /// Adds encryption options from a configuration key or customised options
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="key"></param>
    /// <param name="encryptionRootKey"></param>
    /// <param name="configureOptions"></param>
    /// <exception cref="NullReferenceException"></exception>
    /// <returns></returns>
    public static IDictionaryBuilder<string, Func<IServiceProvider, IEncryptionOptions>>
        AddConfiguration(this IDictionaryBuilder<string, Func<IServiceProvider, IEncryptionOptions>> dictionary, string key, string? encryptionRootKey  = null, Func<IConfiguration, IEncryptionOptions>? configureOptions = null)
    {
        dictionary.Add(key, e => {
            var configuration = e.GetRequiredService<IConfiguration>();
            
            if(configureOptions != null)
            {
                return configureOptions(configuration);
            }
            
            if (!string.IsNullOrWhiteSpace(encryptionRootKey))
            {
                return configuration
                    .GetSection(encryptionRootKey)
                    .Get<IEncryptionOptions>() ?? throw new NullReferenceException("Unable to bind configuration"); ;
            }
            else
                return configuration.Get<IEncryptionOptions>() ?? throw new NullReferenceException("Unable to bind configuration");
        });
        return dictionary;
    }
}
