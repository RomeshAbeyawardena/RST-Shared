using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using RST.Extensions;

namespace RST.AspNetCore.Extensions;

/// <summary>
/// Represents an API Controller
/// </summary>
[ApiController]
public abstract class ApiController
{
    /// <summary>
    /// A base API url
    /// </summary>
    public const string DEFAULT_API_URL = "api";

    /// <summary>
    /// Instance of <see cref="IMediator"/> injected
    /// </summary>
    [Inject]
    protected IMediator Mediator { get; set; }

    /// <summary>
    /// Instance of <see cref="IMapper"/> injected
    /// </summary>
    [Inject]
    protected IMapper Mapper { get; set; }

    /// <summary>
    /// Initializes an inherited instance of <see cref="ApiController"/> 
    /// </summary>
    /// <param name="serviceProvider"></param>
    protected ApiController(IServiceProvider serviceProvider)
    {
        Mediator = null!;
        Mapper = null!;

        var apiControllerType = GetType();
        var properties = apiControllerType.GetAllProperties().Where(p => p.CanWrite && p.HasAttribute(typeof(InjectAttribute), out var attribute));
        
        foreach (var property in properties)
        {
            var service = serviceProvider.GetService(property.PropertyType);

            if (service != null)
            {
                property.SetValue(this, service);
            }
        }
    }
}
