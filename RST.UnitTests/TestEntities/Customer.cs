namespace RST.UnitTests.TestEntities;
/// <summary>
/// 
/// </summary>
public record Customer
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Firstname { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Middlename { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Lastname { get; set; }

}
