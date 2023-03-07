using RST.Contracts;
using RST.Extensions;
using System.Reflection;

namespace RST;


/// <inheritdoc cref="IServiceDefinitionOptions"/>
public record ServiceDefinitionOptions : IServiceDefinitionOptions
{
    private bool configureCoreServices;
    private bool configureCryptographyExtensions;

    internal bool hasChanged = false;

    private IEnumerable<Assembly>? assemblies;

    /// <inheritdoc cref="IServiceDefinitionOptions.ConfigureCoreServices"/>
    public bool ConfigureCoreServices { 
        get => configureCoreServices; 
        set { 
            hasChanged = configureCoreServices != value; 
            configureCoreServices = value; 
        } 
    }

    /// <inheritdoc cref="IServiceDefinitionOptions.ConfigureCryptographyExtensions"/>
    public bool ConfigureCryptographyExtensions { 
        get => configureCryptographyExtensions; 
        set
        {
            hasChanged = configureCryptographyExtensions != value;
            configureCryptographyExtensions = value;
        }
    }

    internal bool HasAssemblies => Assemblies.Any();

    internal IEnumerable<Assembly> Assemblies => assemblies == null || hasChanged 
        ? GetAssemblies()
        : assemblies;

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
        hasChanged = false;
        return assemblies = assemblyList.LoadAssemblies();
    }
}
