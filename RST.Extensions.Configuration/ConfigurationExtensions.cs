using Microsoft.Extensions.Configuration;
using System.Configuration;

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
    /// <param name="path"></param>
    /// <returns></returns>
    public static IConfigurationSection? GetFromPath(this IConfiguration configuration, string path)
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
