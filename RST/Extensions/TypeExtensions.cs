namespace RST.Extensions;

/// <summary>
/// Extensions for <see cref="Type"/>
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Gets a <see cref="List{T}"/> of inherited <see cref="Type"/>s for <paramref name="type"/>
    /// </summary>
    /// <param name="type"><see cref="Type"/> to retrieve inherited types</param>
    /// <returns></returns>
    public static IEnumerable<Type> GetInheritedTypes(this Type type)
    {
        var inheritedTypes = new List<Type>();
        var objectType = typeof(object);
        var currentType = type;
        while (currentType != null && currentType != objectType)
        {
            inheritedTypes.Add(currentType);
            currentType = currentType.BaseType;
        }

        return inheritedTypes;
    }
}
