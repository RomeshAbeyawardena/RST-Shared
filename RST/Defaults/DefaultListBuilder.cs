using RST.Contracts;
using System.Collections;

namespace RST.Defaults;

/// <summary>
/// <inheritdoc cref="IListBuilder{T}"/>
/// </summary>
public class DefaultListBuilder<T> : IListBuilder<T>
{
    private List<T> list;

    private List<T> CopyEntries(List<T> list)
    {
        if (CopyEntriesFromPreviousList && list.Any())
        {
            foreach (var item in this.list)
            {
                list.Add(item);
            }
        }

        return list;
    }

    /// <summary>
    /// Initialises a list builder
    /// </summary>
    /// <param name="list"></param>
    public DefaultListBuilder(List<T>? list = null) 
    {
        this.list = list ?? new List<T>();
    }

    /// <summary>
    /// <inheritdoc cref="IListBuilder{T}.CopyEntriesFromPreviousList"/>
    /// </summary>
    public bool CopyEntriesFromPreviousList { get; set; }

    /// <summary>
    /// <inheritdoc cref="IListBuilder{T}.List"/>
    /// </summary>
    public List<T> List { set => list = CopyEntries(value); }

    /// <summary>
    /// <inheritdoc cref="IListBuilder{T}.Add(T)"/>
    /// </summary>
    public IListBuilder<T> Add(T item)
    {
        list.Add(item);
        return this;
    }

    /// <summary>
    /// <inheritdoc cref="IListBuilder{T}.AddRange(IEnumerable{T})"/>
    /// </summary>
    public IListBuilder<T> AddRange(IEnumerable<T> items)
    {
        list.AddRange(items);
        return this;
    }

    /// <summary>
    /// <inheritdoc cref="List{T}.GetEnumerator"/>
    /// </summary>
    public IEnumerator<T> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    /// <summary>
    /// <inheritdoc cref="IListBuilder{T}.Insert(int, T)"/>
    /// </summary>
    public IListBuilder<T> Insert(int index, T item)
    {
        list.Insert(index, item);
        return this;
    }

    /// <summary>
    /// <inheritdoc cref="IListBuilder{T}.Remove(T)"/>
    /// </summary>
    public IListBuilder<T> Remove(T item)
    {
        list.Remove(item);
        return this;
    }

    /// <summary>
    /// <inheritdoc cref="IListBuilder{T}.RemoveAtIndex(int)"/>
    /// </summary>
    public IListBuilder<T> RemoveAtIndex(int index)
    {
        list.RemoveAt(index);
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
