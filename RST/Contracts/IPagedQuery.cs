using RST.Enumerations;

namespace RST.Contracts;

public interface IPagedQuery<T>
    where T : struct
{
    T? PageIndex { get; set; }
    T? TotalItemsPerPage { get; set; }
    string? OrderByField { get; set; }
    SortOrder? SortOrder { get; set; }
}

public interface IPagedQuery : IPagedQuery<int>
{

}