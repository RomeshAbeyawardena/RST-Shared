using RST.Contracts;

namespace RST.Extensions;

/// <summary>
/// Represents an <see cref="EnumExtensions"/> helper class
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Attempts to parse <typeparamref name="TEnum"/> from <paramref name="model"/> or returns <paramref name="defaultEnumValue"/>
    /// </summary>
    /// <typeparam name="TEnum">The type of <see cref="Enum"/> to parse</typeparam>
    /// <param name="model">Lookup value to containing <typeparamref name="TEnum"/></param>
    /// <param name="defaultEnumValue">Default value to return if parsing was unsuccessful</param>
    /// <returns>Parsed <typeparamref name="TEnum"/> or value of <paramref name="defaultEnumValue"/></returns>
    public static TEnum ParseOrDefault<TEnum>(
        this ILookupValue? model,
        TEnum? defaultEnumValue = default)
        where TEnum : struct
    {
        if (model != null
            && !string.IsNullOrWhiteSpace(model.Name)
            && System.Enum.TryParse<TEnum>(model.Name, out var areaType))
        {
            return areaType;
        }

        return defaultEnumValue ?? default;
    } 
}
