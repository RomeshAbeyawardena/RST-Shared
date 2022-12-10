namespace RST.Contracts;

/// <summary>
/// Represents a modified timestamp
/// </summary>
/// <typeparam name="TDateTime">The datatype used by the modified field</typeparam>
public interface IModified<TDateTime>
where TDateTime : struct
{
    /// <summary>
    /// Gets the modification timestamp
    /// </summary>
    TDateTime? Modified { get; set; }
}
