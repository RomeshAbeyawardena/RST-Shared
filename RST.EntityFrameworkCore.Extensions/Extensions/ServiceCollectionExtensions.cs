using RST.Contracts;
using RST.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RST.EntityFrameworkCore.Extensions;
/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the given <typeparamref name="TDbContext"/> using <paramref name="configureOptions"/> builder and adds the underlining <see cref="IRepository{T}"/> using the defined generic <paramref name="implementationType"/>
    /// </summary>
    /// <typeparam name="TDbContext">The <see cref="DbContext"/> type to register and extract repository models from</typeparam>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to configure the underlining services with</param>
    /// <param name="implementationType">The generic implementation type to register repository models to</param>
    /// <param name="configureOptions">Configures options to the underlining <see cref="DbContextOptionsBuilder"/></param>
    /// <returns></returns>
    public static IServiceCollection AddDbContextAndRepositories<TDbContext>(this IServiceCollection services, Type implementationType, Action<DbContextOptionsBuilder> configureOptions)
        where TDbContext : DbContext
    {
        return services
            .AddDbContextPool<TDbContext>(configureOptions)
            .AddRepositories<TDbContext>(implementationType);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="implementationType"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddDbContextAndRepositories<TDbContext>(this IServiceCollection services, Type implementationType, Action<IServiceProvider, DbContextOptionsBuilder> configureOptions)
        where TDbContext : DbContext
    {
        return services
            .AddDbContextPool<TDbContext>(configureOptions)
            .AddRepositories<TDbContext>(implementationType);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="implementationType"></param>
    /// <param name="modelTypes"></param>
    /// <param name="useScopedServices"></param>
    /// <returns></returns>
    internal static IServiceCollection AddRepositories<TDbContext>(this IServiceCollection services, Type implementationType, IEnumerable<Type> modelTypes, bool useScopedServices = true)
        where TDbContext : DbContext
    {
        var serviceType = typeof(IRepository<>);
        var specificServiceType = typeof(IEntityFrameworkRepository<,>);
        var dbContextType = typeof(TDbContext);
        foreach(var modelType in modelTypes)
        {
            var specificImplementationType = implementationType.MakeGenericType(modelType);

            if (useScopedServices)
            {
                services.AddScoped(serviceType.MakeGenericType(modelType), specificImplementationType);

                services.AddScoped(specificServiceType.MakeGenericType(dbContextType, modelType), specificImplementationType);
            }
            else
            {
                services.AddTransient(serviceType.MakeGenericType(modelType), specificImplementationType);

                services.AddTransient(specificServiceType.MakeGenericType(dbContextType, modelType), specificImplementationType);
            }
        }

        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="implementationType"></param>
    /// <returns></returns>
    internal static IServiceCollection AddRepositories<TDbContext>(this IServiceCollection services, Type implementationType)
        where TDbContext : DbContext
    {
        var dbContextType = typeof(TDbContext);

        var models = dbContextType.GetDbSetModels();

        return services.AddRepositories<TDbContext>(implementationType, models);
    }
}
