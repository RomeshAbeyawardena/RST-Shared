using System.Security.Cryptography;

namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface IModelHasher
{
    /// <summary>
    /// Gets the order index
    /// </summary>
    int OrderIndex { get; }
    /// <summary>
    /// Gets a value to determine whether this is the default model hasher
    /// </summary>
    bool IsDefault { get; }
    /// <summary>
    /// Gets the default options
    /// </summary>
    object? DefaultOptions { get; }
    /// <summary>
    /// Gets the model hasher's name to be identified within a factory
    /// </summary>
    string Name { get; }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="hash"></param>
    /// <param name="otherHash"></param>
    /// <returns></returns>
    bool CompareHash(string hash, string otherHash);
    /// <summary>
    /// Calculates the hash of <typeparamref name="T"/> using the provided <paramref name="model"/>
    /// </summary>
    /// <typeparam name="T">Type of <paramref name="model"/></typeparam>
    /// <param name="model">Supplied model</param>
    /// <param name="options"></param>
    /// <returns></returns>
    string CalculateHash<T>(T model, object options);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="options"></param>
    /// <param name="hash"></param>
    /// <returns></returns>
    bool CompareHash<T>(T model, object? options, string? hash);
}

/// <summary>
/// 
/// </summary>
public interface IModelHasher<TModelHasherOptions> : IModelHasher
{
    /// <summary>
    /// 
    /// </summary>
    new TModelHasherOptions? DefaultOptions { get; }

    /// <summary>
    /// Calculates the hash of <typeparamref name="T"/> using the provided <paramref name="model"/>
    /// </summary>
    /// <typeparam name="T">Type of <paramref name="model"/></typeparam>
    /// <param name="model">Supplied model</param>
    /// <param name="options"></param>
    /// <returns></returns>
    string CalculateHash<T>(T model, TModelHasherOptions options);
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <param name="options"></param>
    /// <param name="hash"></param>
    /// <returns></returns>
    bool CompareHash<T>(T model, TModelHasherOptions options, string hash);
}
