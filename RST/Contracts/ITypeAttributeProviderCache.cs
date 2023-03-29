using System.Reflection;

namespace RST.Contracts;
/// <summary>
/// 
/// </summary>
public interface ITypeAttributeProviderCache : IReadOnlyDictionary<Type, IEnumerable<Attribute>>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="properties"></param>
    void AddOrUpdate(Type type, IEnumerable<Attribute> properties);
}
