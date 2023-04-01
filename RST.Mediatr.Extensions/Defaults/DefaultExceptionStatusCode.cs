using RST.Mediatr.Extensions.Contracts;

namespace RST.Mediatr.Extensions.Defaults;

/// <summary>
/// 
/// </summary>
public record DefaultExceptionStatusCode : IExceptionStatusCode
{
    /// <summary>
    /// 
    /// </summary>
    public Type? ExceptionType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string? StatusMessage { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int? StatusCode { get; set; }
}
