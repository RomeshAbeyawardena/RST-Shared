using Microsoft.Extensions.DependencyInjection;

namespace RST.EntityFrameworkCore.Extensions.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Gets the connection string from an application setting instance
    /// </summary>
    /// <typeparam name="TConnectionStringProvider"></typeparam>
    /// <param name="services"></param>
    /// <param name="getConnectionString"></param>
    /// <returns></returns>
    public static string? GetConnectionString<TConnectionStringProvider>(
        this IServiceProvider services, Func<TConnectionStringProvider, string?> getConnectionString)
        where TConnectionStringProvider : notnull
    {
        var connectionStringProvider = services.GetRequiredService<TConnectionStringProvider>();
        return getConnectionString(connectionStringProvider);
    }
}