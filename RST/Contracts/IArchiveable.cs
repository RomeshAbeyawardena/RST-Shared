namespace RST.Contracts;

/// <summary>
/// Represents an archiveable flag
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IArchiveable<T>
    where T: struct
{
    /// <summary>
    /// Gets or sets the date the entry was archived
    /// </summary>
    T? ArchivedDate { get; set; }
}

/// <summary>
/// <inheritdoc cref="IArchiveable{DateTimeOffset}"/>
/// </summary>
public interface IArchiveable : IArchiveable<DateTimeOffset>
{

}
