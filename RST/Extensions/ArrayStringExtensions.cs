using System.Reflection;

namespace RST.Extensions;

/// <summary>
/// A set of string array extension methods
/// </summary>
public static class ArrayStringExtensions
{
    /// <summary>
    /// Load assemblies by fully qualified name.
    /// </summary>
    /// <param name="assemblyNames"></param>
    /// <returns></returns>
    public static IEnumerable<Assembly> LoadAssemblies(this IEnumerable<string> assemblyNames)
    {
        var assemblies = new List<Assembly>();
        foreach (var assemblyName in assemblyNames)
        {
            var assembly = Assembly.Load(assemblyName);
            assemblies.Add(assembly);
        }

        return assemblies;
    }
}
