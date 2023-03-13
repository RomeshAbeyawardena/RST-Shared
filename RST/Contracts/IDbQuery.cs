namespace RST.Contracts;

/// <summary>
/// Represents a DB query
/// </summary>
public interface IDbQuery
{
    /// <summary>
    /// Gets a value determining whether tracking should be disabled
    /// </summary>
    bool? NoTracking { get; set; }
}
