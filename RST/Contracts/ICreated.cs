namespace RST.Contracts;

/// <summary>
/// Represents a creation timestamp
/// </summary>
/// <typeparam name="TDateTime">The datatype used by the created field</typeparam>
public interface ICreated<TDateTime>
    where TDateTime : struct
{
    /// <summary>
    /// Gets the creation timestamp
    /// </summary>
    TDateTime Created { get; set; }
}

/// <summary>
/// 
/// </summary>
public interface ICreated : ICreated<DateTimeOffset>
{

}