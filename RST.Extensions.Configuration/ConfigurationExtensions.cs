using Microsoft.Extensions.Configuration;

namespace RST.Extensions.Configuration;

/// <summary>
/// 
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="rootPath"></param>
    /// <returns></returns>
    public static IEnumerable<IConfigurationSection> GetSections(this IConfiguration configuration, string rootPath)
    {
        var rootSection = configuration.GetFromPath(rootPath);
        return rootSection.GetChildren();
    }
    ///public static IEnumerable<IConfigurationSection>
    /// <summary>
    /// <inheritdoc cref="ConfigurationBinder.Get{T}(IConfiguration)"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configuration"></param>
    /// <param name="rootPath"></param>
    /// <param name="sectionNames"></param>
    /// <returns></returns>
    public static IEnumerable<T?> GetValues<T>(this IConfiguration configuration, string rootPath,
        params string[] sectionNames)
    {
        var configurationSection = GetFromPath(configuration, $"{rootPath}");
        return GetSectionsFromPaths(configuration, sectionNames.Select(p => $"{rootPath}/{p}").ToArray()).Select(s => s.Get<T>());
    }

    /// <summary>
    /// Gets a list of <see cref="IConfigurationSection"/> instances from the supplied <paramref name="paths"/> from supplied <paramref name="configuration"/> 
    /// </summary>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="paths">Paths to extract from <see cref="IConfiguration"/></param>
    /// <returns></returns>
    public static IEnumerable<IConfigurationSection> GetSectionsFromPaths(this IConfiguration configuration, params string[] paths)
    {
        return paths.Select(p => GetFromPath(configuration, p));
    }

    /// <summary>
    /// Traverses through the sections within <paramref name="configuration"/> in order find an instance of <see cref="IConfigurationSection"/> by a given path separated by either a '/' or '\\'
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/> instance to obtain <see cref="IConfigurationSection"/></param>
    /// <param name="path">The path to traverse</param>
    /// <exception cref="EntryPointNotFoundException"></exception>
    /// <returns>An instance of <see cref="IConfigurationSection"/></returns>
    public static IConfigurationSection GetFromPath(this IConfiguration configuration, string path)
    {
        var splitPath = path.Split('/', '\\');

        IConfigurationSection section = configuration.GetSection(splitPath[0]);

        if (!section.Exists())
        {
            throw new EntryPointNotFoundException();
        }

        for (int i = 1; i < splitPath.Length; i++)
        {
            var currentPath = splitPath[i];
            section = section.GetSection(currentPath);
            if (!section.Exists())
            {
                throw new EntryPointNotFoundException($"Unable to find {currentPath}");
            }
        }

        return section;
    }
}
