namespace RST.Extensions;

/// <summary>
/// 
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Performs the specified action on each element of <typeparamref name="TResult"/>
    /// </summary>
    /// <typeparam name="TRequest">The type of input object</typeparam>
    /// <typeparam name="TResult">The type of output object</typeparam>
    /// <param name="items">Items to perform the specified action on</param>
    /// <param name="itemAction">The action to perform on each item</param>
    /// <returns>An instance of <typeparamref name="TResult"/> returned by <paramref name="itemAction"/></returns>
    public static IEnumerable<TResult> ForEach<TRequest, TResult>(this IEnumerable<TRequest> items,
        Func<TRequest, TResult> itemAction)
    {
        return items.Select(itemAction);
    }

    /// <summary>
    /// Performs the specified action on each element of <typeparamref name="TResult"/>
    /// </summary>
    /// <typeparam name="TRequest">The type of input object</typeparam>
    /// <typeparam name="TResult">The type of output object</typeparam>
    /// <param name="items">Items to perform the specified action on</param>
    /// <param name="itemAction">The action to perform on each item</param>
    /// <returns>An instance of <typeparamref name="TResult"/> returned by <paramref name="itemAction"/></returns>
    public static IEnumerable<TResult> ForEach<TRequest, TResult>(this IEnumerable<TRequest> items,
        Func<int, TRequest, TResult> itemAction)
    {
        return items.Select((i,c) => itemAction(c, i));
    }

    /// <summary>
    /// Performs the specified action on each element of <typeparamref name="TResult"/>
    /// </summary>
    /// <typeparam name="TRequest">The type of input object</typeparam>
    /// <typeparam name="TResult">The type of output object</typeparam>
    /// <param name="items">Items to perform the specified action on</param>
    /// <param name="itemAction">The action to perform on each item</param>
    /// <param name="cancellationToken">The cancellation token used to cancel the async operation</param>
    /// <returns>An instance of <typeparamref name="TResult"/> returned by <paramref name="itemAction"/></returns>
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
