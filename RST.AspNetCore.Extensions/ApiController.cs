using Microsoft.AspNetCore.Mvc;

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
}
