namespace RST.Mediatr.Extensions.Contracts;

/// <summary>
/// 
/// </summary>
public interface IExceptionStatusCode
{
    /// <summary>
    /// 
    /// </summary>
    Type ExceptionType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    string? StatusMessage { get; set; }
    /// <summary>
    /// 
    /// </summary>
    int? StatusCode { get; set; }
}
