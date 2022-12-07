namespace RST.Contracts;

public interface ILookupValue : IIdentity, ICreated<DateTimeOffset>, IModified<DateTimeOffset>
{
    string? Name { get; set; }
    string? DisplayName { get; set; }
    bool Default { get; set; }
}
