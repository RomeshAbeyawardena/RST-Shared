using RST.Contracts;
using RST.Enumerations;

namespace RST.Security.Cryptography.Extensions.Defaults;

internal class DefaultEncryptionModuleOptions : IEncryptionModuleOptions
{

    public DefaultEncryptionModuleOptions(EncryptionCaseConvention encryptionCaseConvention)
    {
        EncryptionOptionsFactory = new Dictionary<string, Func<IServiceProvider, IEncryptionOptions>>();

        EncryptionOptions = new Dictionary<string, IEncryptionOptions>();
        EncryptionCaseConvention = encryptionCaseConvention;
    }

    public IDictionary<string, Func<IServiceProvider, IEncryptionOptions>> EncryptionOptionsFactory { get; }
    public IDictionary<string, IEncryptionOptions> EncryptionOptions { get; }
    public EncryptionCaseConvention EncryptionCaseConvention { get; set; }
}
