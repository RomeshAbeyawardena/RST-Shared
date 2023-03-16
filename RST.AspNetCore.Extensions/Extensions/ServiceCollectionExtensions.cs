using Microsoft.Extensions.DependencyInjection;
using RST.AspNetCore.Extensions.Contracts;

namespace RST.AspNetCore.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds an application authentication repository to the supplied instance of <see cref="IServiceCollection"/> <paramref name="services"/>
    /// </summary>
    /// <typeparam name="TApplicationAuthenticationRepository"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationAuthenticationRepository<TApplicationAuthenticationRepository>(IServiceCollection services)
        where TApplicationAuthenticationRepository : class, IApplicationAuthenticationRepository
    {
        return services.AddTransient<IApplicationAuthenticationRepository, TApplicationAuthenticationRepository>();
    }
}
