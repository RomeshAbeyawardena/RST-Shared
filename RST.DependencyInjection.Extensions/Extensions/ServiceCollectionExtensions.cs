﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Internal;
using RST.Attributes;
using RST.Contracts;
using RST.Defaults;
using System.Reactive.Subjects;
using System.Reflection;
using System.Security.Cryptography;

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

        return services
            .AddSingleton(c => RSA.Create())
            .AddSingleton<IClockProvider, DefaultClockProvider>()
            .AddSingleton(typeof(ISubject<>), typeof(Subject<>))
            .AddTransient(typeof(IDictionaryBuilder<,>), typeof(DefaultInjectedDictionaryBuilder<,>))
            .AddTransient(typeof(IListBuilder<>), typeof(DefaultInjectedListBuilder<>));
    }

    /// <summary>
    /// Adds services decorated with the <see cref="RegisterAttribute"/> 
    /// </summary>
    /// <param name="services">Instance of <see cref="IServiceCollection"/> to add core service sto</param>
    /// <param name="configureOptions">Configure options</param>
    /// <param name="assemblies">Assemblies used to for scan services</param>
    /// <returns>Passed instance of <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddServicesWithRegisterAttribute(
      this IServiceCollection services,
      Action<IServiceDefinitionOptions>? configureOptions = null,
      params Assembly[] assemblies)
    {
        var serviceDefinitionOptions = new ServiceDefinitionOptions();
        configureOptions?.Invoke(serviceDefinitionOptions);

        if (serviceDefinitionOptions.HasAssemblies)
        {
            assemblies = assemblies.Union(serviceDefinitionOptions.Assemblies).ToArray();
        }

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
