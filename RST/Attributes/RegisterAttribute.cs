using Microsoft.Extensions.DependencyInjection;

namespace RST.Attributes;
/// <summary>
/// Represents an <see cref="Attribute"/> used to decorate services for dependency injection
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RegisterAttribute : Attribute
{

    /// <summary>
    /// Creates an instance of <see cref="RegisterAttribute"/>
    /// </summary>
    /// <param name="serviceLifetime">The service lifetime to be assigned to the service</param>
    public RegisterAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        ServiceLifetime = serviceLifetime;
    }

    /// <summary>
    /// Gets the service lifetime assigned to the service
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; }
}
