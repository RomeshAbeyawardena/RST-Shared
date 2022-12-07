﻿using RST.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RST.Extensions;
public static class ServiceCollectionExtensions
{
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
