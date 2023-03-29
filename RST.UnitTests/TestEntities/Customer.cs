using MemoryPack;
using RST.Contracts;

namespace RST.UnitTests.TestEntities;
/// <summary>
/// 
/// </summary>
[MemoryPackable]
public partial record Customer : IIdentity
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

    public DateTimeOffset? PopulatedDate { get;set; }

}
