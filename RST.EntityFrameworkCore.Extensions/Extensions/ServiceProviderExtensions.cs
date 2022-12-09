using Microsoft.Extensions.DependencyInjection;

namespace RST.EntityFrameworkCore.Extensions.Extensions;

public static class ServiceProviderExtensions
{
    public static string? GetConnectionString<TConnectionStringProvider>(
        this IServiceProvider services, Func<TConnectionStringProvider, string?> getConnectionString)
        where TConnectionStringProvider : notnull
    {
        var connectionStringProvider = services.GetRequiredService<TConnectionStringProvider>();
        return getConnectionString(connectionStringProvider);
    }
}