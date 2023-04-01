namespace RST.Mediatr.Extensions.Contracts;

/// <summary>
/// 
/// </summary>
public interface IExceptionHandlerExceptionStatusCodeFactory : IReadOnlyDictionary<Type, IExceptionStatusCode>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="statusCode"></param>
    void TryAddExceptionStatusCode(Type type, IExceptionStatusCode statusCode);
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="statusCode"></param>
    void TryAddExceptionStatusCode<TException>(IExceptionStatusCode statusCode);
}
