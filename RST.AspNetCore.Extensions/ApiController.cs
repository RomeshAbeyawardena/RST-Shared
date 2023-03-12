using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RST.DependencyInjection.Extensions;
using RST.Extensions;

namespace RST.AspNetCore.Extensions;

/// <summary>
/// Represents an API Controller
/// </summary>
[ApiController]
public abstract class ApiController : EnableInjectionBase<InjectAttribute>
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
        : base(serviceProvider)
    {
        Mediator = null!;
        Mapper = null!;
        ConfigureInjection();
    }
}
