using Microsoft.Extensions.DependencyInjection;

namespace RST.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RegisterAttribute : Attribute
{
    public RegisterAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        ServiceLifetime = serviceLifetime;
    }

    public ServiceLifetime ServiceLifetime { get; }
}
