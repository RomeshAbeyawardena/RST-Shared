using RST.Contracts;

namespace RST.Defaults;

/// <summary>
/// <inheritdoc cref="DefaultDictionaryBuilder{TKey, TValue}"/> (injected)
/// </summary> 
public class DefaultInjectedDictionaryBuilder<TKey, TValue> : DefaultDictionaryBuilder<TKey, TValue>
    where TKey :notnull
{
}