namespace RST.Extensions;

/// <summary>
/// 
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// <inheritdoc cref="List{TResult}.ForEach(Action{TResult})"/>
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="itemAction"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> ForEach<TRequest, TResult>(this IEnumerable<TRequest> items,
        Func<int, TRequest, TResult> itemAction)
    {
        var resultList = new List<TResult>();
        for (int i = 0; i < items.Count(); i++)
        {
            resultList.Add(
                itemAction(i, items.ElementAt(i)));
        }

        return resultList;
    }

    /// <summary>
    /// <inheritdoc cref="List{TResult}.ForEach(Action{TResult})"/>
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="itemAction"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<TResult>> ForEach<TRequest, TResult>(this IEnumerable<TRequest> items,
        Func<int, TRequest, CancellationToken, Task<TResult>> itemAction, CancellationToken cancellationToken)
    {
        var resultList = new List<TResult>();
        for (int i = 0; i < items.Count(); i++)
        {
            resultList.Add(await
                itemAction(i, items.ElementAt(i), cancellationToken));
        }

        return resultList;
    }
}
