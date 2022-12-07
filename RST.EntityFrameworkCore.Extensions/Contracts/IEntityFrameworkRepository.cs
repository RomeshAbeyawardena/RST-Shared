using RST.Contracts;
using Microsoft.EntityFrameworkCore;

namespace RST.Persistence.Contracts;

public interface IEntityFrameworkRepository<TDbContext, T> : IRepository<T>
    where TDbContext : DbContext
    where T : class
{
    TDbContext Context { get; }
} 