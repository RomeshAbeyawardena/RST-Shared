namespace RST.Contracts;

/// <summary>
/// Represents a list builder
/// </summary>
public interface IListBuilder<T> : IEnumerable<T>
{
    /// <summary>
    /// <inheritdoc cref="List{T}.Add(T)"/>
    /// </summary>
    /// <param name="item"><inheritdoc cref="List{T}.Add(T)"/></param>
    /// <returns>This instance of <see cref="IListBuilder{T}"/></returns>
    IListBuilder<T> Add(T item);
    /// <summary>
    /// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
    /// </summary>
    /// <param name="items"><inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/></param>
    /// <returns>This instance of <see cref="IListBuilder{T}"/></returns>
    IListBuilder<T> AddRange(IEnumerable<T> items);
    /// <summary>
    /// <inheritdoc cref="IList{T}.Insert(int, T)"/>
    /// </summary>
    /// <param name="index"><inheritdoc cref="IList{T}.Insert(int, T)"/></param>
    /// <param name="item"><inheritdoc cref="List{T}.Add(T)"/></param>
    /// <returns>This instance of <see cref="IListBuilder{T}"/></returns>
    IListBuilder<T> Insert(int index, T item);
    /// <summary>
    /// <inheritdoc cref="List{T}.Remove(T)"/>
    /// </summary>
    /// <param name="item"><inheritdoc cref="List{T}.Remove(T)"/></param>
    /// <returns>This instance of <see cref="IListBuilder{T}"/></returns>
    IListBuilder<T> Remove(T item);
    /// <summary>
    /// <inheritdoc cref="IList{T}.RemoveAt(int)"/>
    /// </summary>
    /// <param name="index"><inheritdoc cref="IList{T}.RemoveAt(int)"/></param>
    /// <returns>This instance of <see cref="IListBuilder{T}"/></returns>
    IListBuilder<T> RemoveAtIndex(int index);

    /// <summary>
    /// Gets or sets a value to determine whether the entries in the previous list reference should be copied over to the new reference.
    /// </summary>
    bool CopyEntriesFromPreviousList { get; set; }

    /// <summary>
    /// Sets the underlining referenced list
    /// </summary>
    List<T> List { set; }
}
