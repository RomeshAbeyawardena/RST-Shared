using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        services.AddHttpContextAccessor()
            .TryAddScoped(s => s.GetRequiredService<IHttpContextAccessor>().HttpContext!.Request.Headers);
        return services.AddTransient<IApplicationAuthenticationRepository, TApplicationAuthenticationRepository>();
    }
}
