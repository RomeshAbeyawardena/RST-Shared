using Microsoft.AspNetCore.Mvc;

namespace RST.AspNetCore.Extensions;

[ApiController, Route(DEFAULT_API_URL)]
public class ApiController
{
    public const string DEFAULT_API_URL = "/api/{controller}";
}
