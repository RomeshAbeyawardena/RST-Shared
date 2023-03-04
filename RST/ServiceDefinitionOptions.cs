using RST.Extensions;
using System.Reflection;

namespace RST;

/// <summary>
/// Represents service definition options
/// </summary>
public class ServiceDefinitionOptions
{
    private IEnumerable<Assembly>? assemblies;
    /// <summary>
    /// Configures core services
    /// </summary>
    public bool ConfigureCoreServices { get; set; }
    /// <summary>
    /// Configures cryptography extensions
    /// </summary>
    public bool ConfigureCryptographyExtensions { get; set; }

    internal bool HasAssemblies => Assemblies.Any();

    internal IEnumerable<Assembly> Assemblies => assemblies ?? GetAssemblies();

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

        return assemblies = assemblyList.LoadAssemblies();
    }
}
