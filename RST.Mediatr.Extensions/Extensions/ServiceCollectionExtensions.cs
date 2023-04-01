using Microsoft.Extensions.DependencyInjection;
using RST.Mediatr.Extensions.Contracts;
using RST.Mediatr.Extensions.Defaults;

namespace RST.Mediatr.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configurationExceptionHandler"></param>
    /// <param name="addHttpContextAccessor"></param>
    /// <returns></returns>
    public static IServiceCollection AddDefaultExceptionHandler(this IServiceCollection services,
        Action<IExceptionHandlerExceptionStatusCodeFactory> configurationExceptionHandler,
        bool addHttpContextAccessor = true)
    {
        if (addHttpContextAccessor)
        {
            services.AddHttpContextAccessor();
        }

        return services
            .AddSingleton<IExceptionHandlerExceptionStatusCodeFactory>(new DefaultExceptionHandlerExceptionStatusCodeFactory(configurationExceptionHandler))
            .AddSingleton(
                typeof(MediatR.Pipeline.IRequestExceptionHandler<,,>),
                typeof(DefaultExceptionHandler<,,>));
    }
}
