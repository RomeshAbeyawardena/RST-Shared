using RST.Contracts;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace RST.Defaults;

/// <inheritdoc cref="IDictionaryBuilder{TKey, TValue}"/>
public class DefaultDictionaryBuilder<TKey, TValue> : IDictionaryBuilder<TKey, TValue>
    where TKey: notnull
{
    private readonly IDictionary<TKey, TValue> dictionary;

    /// <summary>
    /// Initialises an instance of <see cref="IDictionaryBuilder{TKey, TValue}"/>
    /// </summary>
    /// <param name="dictionary"></param>
    public DefaultDictionaryBuilder(IDictionary<TKey, TValue>? dictionary = null)
    {
        this.dictionary = dictionary ?? new ConcurrentDictionary<TKey, TValue>();
    }

    /// <inheritdoc cref="IDictionary{TKey, TValue}"/>
    public TValue this[TKey key] => dictionary[key];

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Keys"/>
    public IEnumerable<TKey> Keys => dictionary.Keys;

    /// <inheritdoc cref="IDictionary{TKey, TValue}.Values"/>
    public IEnumerable<TValue> Values => dictionary.Values;

    /// <inheritdoc cref="Dictionary{TKey, TValue}.Count"/>
    public int Count => dictionary.Count;

    /// <inheritdoc cref="IDictionaryBuilder{TKey, TValue}.Add(KeyValuePair{TKey, TValue})"/>
    public IDictionaryBuilder<TKey, TValue> Add(TKey key, TValue value)
    {
        dictionary.Add(key, value);
        return this;
    }
    /// <inheritdoc cref="IDictionaryBuilder{TKey, TValue}.Add(KeyValuePair{TKey, TValue})"/>
    public IDictionaryBuilder<TKey, TValue> Add(KeyValuePair<TKey, TValue> item)
    {
        return Add(item.Key, item.Value);
    }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.ContainsKey(TKey)"/>
    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.Count"/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue(TKey, out TValue)"/>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    /// <inheritdoc cref="Dictionary{TKey, TValue}.GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }
}
