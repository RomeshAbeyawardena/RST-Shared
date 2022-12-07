namespace RST.Contracts;

public interface ICreated<TDateTime>
    where TDateTime : struct
{
    TDateTime Created { get; set; }
}
