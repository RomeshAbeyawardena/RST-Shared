using RST.Attributes;
using RST.Contracts;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RST.Defaults;

/// <summary>
/// 
/// </summary>
[Register]
public class PropertyTypeProviderCache : IPropertyTypeProviderCache
{
    private readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> propertiesDictionary;
    /// <summary>
    /// 
    /// </summary>
    public PropertyTypeProviderCache()
    {
        propertiesDictionary = new ConcurrentDictionary<Type, IEnumerable<PropertyInfo>>();
    }

    /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}"/>
    public IEnumerable<PropertyInfo> this[Type key] => propertiesDictionary[key];

    /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.Keys"/>
    public IEnumerable<Type> Keys => propertiesDictionary.Keys;
    /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.Values"/>
    public IEnumerable<IEnumerable<PropertyInfo>> Values => propertiesDictionary.Values;
    /// <inheritdoc cref="IReadOnlyCollection{T}.Count"/>
    public int Count => propertiesDictionary.Count;
    /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.ContainsKey(TKey)"/>
    public bool ContainsKey(Type key)
    {
        return propertiesDictionary.ContainsKey(key);
    }
    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator()"/>
    public IEnumerator<KeyValuePair<Type, IEnumerable<PropertyInfo>>> GetEnumerator()
    {
        return propertiesDictionary.GetEnumerator();
    }

    /// <summary>
    /// Performs an add or update 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="properties"></param>
    public void AddOrUpdate(Type type, IEnumerable<PropertyInfo> properties)
    {
        if(!propertiesDictionary.TryAdd(type, properties))
        {
            propertiesDictionary[type] = properties;
        }
    }

    /// <inheritdoc cref="IReadOnlyDictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
    public bool TryGetValue(Type key, [MaybeNullWhen(false)] out IEnumerable<PropertyInfo> value)
    {
        return TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
