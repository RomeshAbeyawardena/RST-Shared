using MessagePack;
using RST.Contracts;

namespace RST.UnitTests.TestEntities;
/// <summary>
/// 
/// </summary>
[MessagePackObject(true)]
public partial record Customer : IIdentity, IHashable
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
    /// <summary>
    /// 
    /// </summary>
    [IgnoreMember]
    public string? Hash { get; set; }
}
