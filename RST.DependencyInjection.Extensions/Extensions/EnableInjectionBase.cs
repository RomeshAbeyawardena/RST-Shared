using Microsoft.Extensions.DependencyInjection;
using RST.Contracts;
using RST.Defaults;
using RST.Extensions;

namespace RST.DependencyInjection.Extensions;

/// <summary>
/// Enables injection for the inherited class 
/// </summary>
public abstract class EnableInjectionBase<TInjectAttribute>
    where TInjectAttribute : Attribute
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Configures injection for fields and properties in an instance of the inherited class
    /// </summary>
    protected void ConfigureInjection()
    {
        var typeProviderCache = serviceProvider.GetService<IPropertyTypeProviderCache>()
            ?? throw new NullReferenceException($"{nameof(PropertyTypeProviderCache)} not found");


        var instanceType = GetType();

        var properties = instanceType.GetAllProperties(typeProviderCache);

        var injectableProperties = properties.Where(p => p.CanWrite && p.Property.HasAttribute(typeof(TInjectAttribute), out var attribute));

        foreach (var property in injectableProperties)
        {
            var service = serviceProvider.GetService(property.Type);

            if (service != null)
            {
                property.SetValue(this, service);
            }
        }
    }

    /// <summary>
    /// Initialses an instance of the inherited class
    /// </summary>
    /// <param name="serviceProvider"></param>
    public EnableInjectionBase(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
}
