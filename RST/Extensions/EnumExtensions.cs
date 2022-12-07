using RST.Contracts;

namespace RST.Extensions;

public static class Enum
{
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
