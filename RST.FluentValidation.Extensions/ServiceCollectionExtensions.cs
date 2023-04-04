using FluentValidation;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RST.FluentValidation.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddValidationHandler(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            return services
                .AddValidatorsFromAssemblies(assemblies)
                .AddScoped(typeof(IRequestPreProcessor<>), typeof(FluentValidationRequestHandler<>));
        }
    }
}
