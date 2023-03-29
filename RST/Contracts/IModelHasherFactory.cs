namespace RST.Contracts;

/// <summary>
/// 
/// </summary>
public interface IModelHasherFactory
{
    /// <summary>
    /// Gets a model hasher by name
    /// </summary>
    /// <param name="modelHasher"></param>
    /// <returns></returns>
    IModelHasher? GetModelHasher(string modelHasher);
}
