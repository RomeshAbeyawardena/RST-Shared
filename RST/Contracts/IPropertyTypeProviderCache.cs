using System.Reflection;

namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface IPropertyTypeProviderCache : IReadOnlyDictionary<Type, IEnumerable<PropertyInfo>>
{
}
