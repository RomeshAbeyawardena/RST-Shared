namespace RST.Contracts;

/// <summary>
/// Represents a Db command
/// </summary>
public interface IDbCommand
{
    /// <summary>
    /// Gets or sets a value that determines whether changes should be committed once the command has completed successfully
    /// </summary>
    bool CommitChanges { get; set; }
}
