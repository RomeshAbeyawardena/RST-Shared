using RST.Mediatr.Extensions.Contracts;
using RST.Mediatr.Extensions.Defaults;

namespace RST.Mediatr.Extensions;

/// <summary>
/// 
/// </summary>
public static class ExceptionHandlerExceptionStatusCodeFactoryExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="factory"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IExceptionHandlerExceptionStatusCodeFactory TryAddExceptionStatusCode<T>(this IExceptionHandlerExceptionStatusCodeFactory factory,
        Action<IExceptionStatusCode> configure)
    {
        var defaultExceptionStatusCode = new DefaultExceptionStatusCode();
        configure(defaultExceptionStatusCode);
        return factory.TryAddExceptionStatusCode<T>(defaultExceptionStatusCode);
    }
}
