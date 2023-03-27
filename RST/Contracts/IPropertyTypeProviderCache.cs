using System.Reflection;

namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface IPropertyTypeProviderCache : IReadOnlyDictionary<Type, IEnumerable<PropertyInfo>>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="properties"></param>
    void AddOrUpdate(Type type, IEnumerable<PropertyInfo> properties);
}
