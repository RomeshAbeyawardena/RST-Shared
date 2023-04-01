using MediatR;
using RST.Contracts;
using RST.Extensions;

namespace RST.Mediatr.Extensions;

/// <summary>
/// 
/// </summary>
public static class RepositoryHandlerBaseExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="repositoryHandler"></param>
    /// <param name="model"></param>
    /// <param name="modifiedModel"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static bool ValidateHash<TRequest, TResponse, TModel>(
        this RepositoryHandlerBase<TRequest, TResponse, TModel> repositoryHandler,
        TModel model, TModel modifiedModel, object? options = null)
        where TRequest : IRequest<TResponse>
        where TModel : class
    {
        var propertyModelHasherImplementations = model.GetModelHashers(repositoryHandler.PropertyTypeProviderCache!, repositoryHandler.ModelHasherFactory!);

        var truthTable = new List<bool>();
        foreach (var (prop, implementation) in propertyModelHasherImplementations)
        {
            if (implementation == null)
            {
                continue;
            }

            var modifiedValue = prop.GetValue(modifiedModel)?.ToString();
            var originalValue = prop.GetValue(model)?.ToString();
            
            if(originalValue == null || modifiedValue == null) {
                truthTable.Add(false);
                continue;
            }

            truthTable.Add(
                implementation.CompareHash(modifiedValue, originalValue));
        }

        return !truthTable.Any() || truthTable.All(a => a);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    /// <param name="removeArchivedFlag"></param>
    /// <param name="repositoryHandler"></param>
  
    /// <param name="model"></param>
    public static bool SetArchived<TRequest, TResponse, TModel>(this RepositoryHandlerBase<TRequest, TResponse, TModel> repositoryHandler,
        TModel model, bool removeArchivedFlag = false)
        where TRequest : IRequest<TResponse>
        where TModel : class
    {
        if(model is IArchiveable archiveable)
        {
            archiveable.ArchivedDate = removeArchivedFlag 
                ? null 
                : repositoryHandler.ClockProvider!.UtcNow;

            return true;
        }

        return false;
    }
}
