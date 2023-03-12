using RST.DependencyInjection.Extensions.Attributes;
using RST.Extensions;

namespace RST.DependencyInjection.Extensions;

/// <summary>
/// Enables injection for the inherited class 
/// </summary>
public abstract class EnableInjectionBase<TInjectAttribute>
    where TInjectAttribute: Attribute
{
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Configures injection for fields and properties in an instance of the inherited class
    /// </summary>
    protected void ConfigureInjection()
    {
        var instanceType = GetType();
        var properties = instanceType.GetAllProperties().Where(p => p.CanWrite && p.HasAttribute(typeof(TInjectAttribute), out var attribute));

        foreach (var property in properties)
        {
            var service = serviceProvider.GetService(property.PropertyType);

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
