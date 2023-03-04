using RST.Extensions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace RST;

/// <summary>
/// Represents service definition options
/// </summary>
public class ServiceDefinitionOptions
{
    /// <summary>
    /// Configures core services
    /// </summary>
    public bool ConfigureCoreServices { get; set; }
    /// <summary>
    /// Configures cryptography extensions
    /// </summary>
    public bool ConfigureCryptographyExtensions { get; set; }

    internal IEnumerable<Assembly> GetAssemblies()
    {
        var assemblyList = new List<string>();

        if (ConfigureCoreServices)
        {
            assemblyList.Add(ServiceDefinitions.CORE_ASSEMBLY);
        }

        if (ConfigureCryptographyExtensions)
        {
            assemblyList.Add(ServiceDefinitions.RST_SECURITY_CRYPTOGRAPHY_EXTENSIONS);
        }

        return assemblyList.LoadAssemblies();
    }
}

/// <summary>
/// Represents service definition
/// </summary>
public static class ServiceDefinitions
{
    /// <summary>
    /// The core assembly
    /// </summary>
    public const string CORE_ASSEMBLY = "RST";
    /// <summary>
    /// The RST.Security.Cryptography.Extensions assembly
    /// </summary>
    public const string RST_SECURITY_CRYPTOGRAPHY_EXTENSIONS = "RST.Security.Cryptography.Extensions";
}
