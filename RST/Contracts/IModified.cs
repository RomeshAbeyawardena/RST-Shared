namespace RST.Contracts;

public interface IModified<TDateTime>
where TDateTime : struct
{
    TDateTime? Modified { get; set; }
}
