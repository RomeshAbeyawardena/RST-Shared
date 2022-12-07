namespace RST.Contracts;

public interface IIdentity<TKey>
    where TKey : struct 
{
    TKey Id { get; set; }
}

public interface IIdentity : IIdentity<Guid>
{
    
}
