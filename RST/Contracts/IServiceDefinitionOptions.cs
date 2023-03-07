namespace RST.Contracts;

/// <summary>
/// Represents service definition options
/// </summary>
public interface IServiceDefinitionOptions
{
    /// <summary>
    /// Configures core services
    /// </summary>
    bool ConfigureCoreServices
    {
        get; set;
    }

    /// <summary>
    /// Configures cryptography extensions
    /// </summary>
    bool ConfigureCryptographyExtensions
    {
        get; set;
    }
}