using RST.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Internal;
using RST.Contracts;
using RST.Defaults;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RST.DependencyInjection.Extensions;
/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds core services to an instance of <see cref="IServiceCollection"/>
    /// </summary>
    /// <param name="services">Instance of <see cref="IServiceCollection"/> to add core service sto</param>
    /// <returns>Passed instance of <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services
            .TryAddSingleton<ISystemClock, SystemClock>();

        return services.AddSingleton<IClockProvider, DefaultClockProvider>();
    }

    /// <summary>
    /// Adds services decorated with the <see cref="RegisterAttribute"/> 
    /// </summary>
    /// <param name="services">Instance of <see cref="IServiceCollection"/> to add core service sto</param>
    /// <param name="assemblies">Assemblies used to for scan services</param>
    /// <returns>Passed instance of <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddServicesWithRegisterAttribute(
      this IServiceCollection services,
      params Assembly[] assemblies)
    {
        return services.Scan(s => s
            .FromAssemblies(assemblies)
            .AddClasses(c => c.WithAttribute<RegisterAttribute>(c => c.ServiceLifetime == ServiceLifetime.Singleton))
            .AsSelfWithInterfaces()
            .WithSingletonLifetime()
            .AddClasses(c => c.WithAttribute<RegisterAttribute>(c => c.ServiceLifetime == ServiceLifetime.Transient))
            .AsSelfWithInterfaces()
            .WithTransientLifetime()
            .AddClasses(c => c.WithAttribute<RegisterAttribute>(c => c.ServiceLifetime == ServiceLifetime.Scoped))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
    }
}
