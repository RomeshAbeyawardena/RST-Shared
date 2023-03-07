namespace RST.Contracts;

/// <summary>
/// Represents a dictionary builder
/// </summary>
public interface IDictionaryBuilder<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    /// <inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
    /// <returns>This instance of <see cref="IDictionaryBuilder{TKey, TValue}" /></returns>
    IDictionaryBuilder<TKey, TValue> Add(TKey key, TValue value);

    /// <summary>
    /// <inheritdoc cref="Dictionary{TKey, TValue}.Add(TKey, TValue)"/>
    /// </summary>
    /// <param name="item">Adds a key value pair of <typeparamref name="TKey"/> and <typeparamref name="TValue"/></param>
    /// <returns>This instance of <see cref="IDictionaryBuilder{TKey, TValue}" /></returns>
    IDictionaryBuilder<TKey, TValue> Add(KeyValuePair<TKey, TValue> item);

    /// <summary>
    /// Gets or sets a value to determine whether the entries in the previous dictionary reference should be copied over to the new reference.
    /// </summary>
    bool CopyEntriesFromPreviousDictionary { get; set; }

    /// <summary>
    /// Sets the underlining referenced dictionary
    /// </summary>
    IDictionary<TKey, TValue> Dictionary {set; }
}
