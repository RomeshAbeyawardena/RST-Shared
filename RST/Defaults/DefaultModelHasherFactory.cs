using RST.Attributes;
using RST.Contracts;

namespace RST.Defaults;

/// <summary>
/// 
/// </summary>
[Register]
public class DefaultModelHasherFactory : IModelHasherFactory
{
    private readonly IEnumerable<IModelHasher> modelHashers;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelHashers"></param>
    public DefaultModelHasherFactory(IEnumerable<IModelHasher> modelHashers)
    {
        this.modelHashers = modelHashers;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public IModelHasher GetDefault()
    {
        return modelHashers.Where(m => m.IsDefault).OrderBy(m => m.OrderIndex).FirstOrDefault()
            ?? throw new NullReferenceException("Default implementation not available");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelHasher"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IModelHasher? GetModelHasher(string modelHasher)
    {
        return modelHashers.FirstOrDefault(h => h.Name.Equals(modelHasher, StringComparison.InvariantCultureIgnoreCase));
    }
}
