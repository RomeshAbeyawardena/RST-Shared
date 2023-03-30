namespace RST.Contracts;

/// <summary>
/// Represents an Identity
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IIdentity<TKey>
    where TKey : struct
{
    /// <summary>
    /// Gets the identifier
    /// </summary>
    TKey Id { get; set; }
}

/// <inheritdoc cref="IIdentity{TKey}"/>
public interface IIdentity : IIdentity<Guid>
{

}
