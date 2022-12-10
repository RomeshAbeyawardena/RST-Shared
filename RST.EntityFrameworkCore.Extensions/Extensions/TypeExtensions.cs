namespace RST.EntityFrameworkCore.Extensions;
/// <summary>
/// 
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbSetType"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetDbSetModels(this Type dbSetType)
    {
        var modelTypes = new List<Type>();
        foreach (var property in dbSetType.GetProperties())
        {
            var propertyType = property.PropertyType;
            
            if (!propertyType.IsGenericType)
            {
                continue;
            }

            var genericType = propertyType.GetGenericArguments().Single();

            modelTypes.Add(genericType);
        }

        return modelTypes;
    } 
}
