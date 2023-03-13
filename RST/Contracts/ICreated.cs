namespace RST.Contracts;

/// <summary>
/// Represents a creation timestamp
/// </summary>
/// <typeparam name="TDateTime">The datatype used by the created field</typeparam>
public interface ICreated<TDateTime>
    where TDateTime : struct
{
    /// <summary>
    /// Gets or sets the creation timestamp
    /// </summary>
    TDateTime Created { get; set; }
}

/// <summary>
/// Represents a creation timestamp
/// </summary>
public interface ICreated : ICreated<DateTimeOffset>
{

}