using RST.Contracts;
using Microsoft.EntityFrameworkCore;

namespace RST.Persistence.Contracts;

/// <summary>
/// Represents an Entity framework repository
/// </summary>
/// <typeparam name="TDbContext">The Db Context type to be used by this repository</typeparam>
/// <typeparam name="T">The entity type for the underlining repository</typeparam>
public interface IEntityFrameworkRepository<TDbContext, T> : IRepository<T>
    where TDbContext : DbContext
    where T : class
{
    /// <summary>
    /// Gets an instance of <typeparamref name="TDbContext"/> as an instance of <see cref="DbContext"/>
    /// </summary>
    TDbContext Context { get; }
} 