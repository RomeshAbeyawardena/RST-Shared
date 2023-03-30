﻿using RST.Contracts;

namespace RST;

/// <summary>
/// 
/// </summary>
public abstract class ModelHasherBase<TModelHasherOptions> : IModelHasher<TModelHasherOptions>
{
    object? IModelHasher.DefaultOptions => DefaultOptions;

    /// <inheritdoc cref="IModelHasher{TModelHasherOptions}.DefaultOptions"/>
    public TModelHasherOptions? DefaultOptions { get; protected set; }

    /// <inheritdoc cref="IModelHasher.Name"/>
    public string Name { get; }

    /// <inheritdoc cref="IModelHasher.OrderIndex"/>
    public int OrderIndex { get; }

    /// <inheritdoc cref="IModelHasher.IsDefault"/>
    public bool IsDefault { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isDefault"></param>
    /// <param name="orderIndex"></param>
    /// <param name="defaultOptions"></param>
    public ModelHasherBase(string name, bool isDefault = false,
        int orderIndex = int.MaxValue, TModelHasherOptions? defaultOptions = default)
    {
        Name = name;
        IsDefault = isDefault;
        OrderIndex = orderIndex;
        DefaultOptions = defaultOptions;
    }

    /// <inheritdoc cref="IModelHasher{TModelHasherOptions}.CalculateHash{T}(T, TModelHasherOptions)"/>
    public abstract string CalculateHash<T>(T model, TModelHasherOptions options);

    ///<inheritdoc cref="IModelHasher{TModelHasherOptions}.CompareHash{T}(T, TModelHasherOptions, string)"/>
    public virtual bool CompareHash<T>(T model, TModelHasherOptions options, string hash)
    {
        var otherHash = CalculateHash<T>(model, options);
        return CompareHash(hash, otherHash);
    }

    ///<inheritdoc cref="IModelHasher.CompareHash(string, string)"/>
    public virtual bool CompareHash(string hash, string otherHash)
    {
        return hash.Equals(otherHash);
    }

    string IModelHasher.CalculateHash<T>(T model, object options)
    {
        if (options is TModelHasherOptions opts)
        {
            return CalculateHash<T>(model, opts);
        }

        throw new InvalidOperationException();
    }

    bool IModelHasher.CompareHash<T>(T model, object? options, string? hash)
    {
        if (options is TModelHasherOptions opts)
        {
            return CompareHash<T>(model, opts, hash ?? string.Empty);
        }

        throw new InvalidOperationException();
    }
}
