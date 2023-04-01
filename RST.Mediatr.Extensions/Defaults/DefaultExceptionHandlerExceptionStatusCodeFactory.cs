using RST.Attributes;
using RST.Mediatr.Extensions.Contracts;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace RST.Mediatr.Extensions.Defaults;
/// <summary>
/// 
/// </summary>
public class DefaultExceptionHandlerExceptionStatusCodeFactory : IExceptionHandlerExceptionStatusCodeFactory
{
    private readonly IDictionary<Type, IExceptionStatusCode> exceptionStatusCodes;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configure"></param>
    public DefaultExceptionHandlerExceptionStatusCodeFactory(Action<IExceptionHandlerExceptionStatusCodeFactory> configure)
    {
        this.exceptionStatusCodes = new Dictionary<Type, IExceptionStatusCode>();

        configure(this);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IExceptionStatusCode this[Type key] => exceptionStatusCodes[key];
    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<Type> Keys => exceptionStatusCodes.Keys;
   /// <summary>
   /// 
   /// </summary>
    public IEnumerable<IExceptionStatusCode> Values => exceptionStatusCodes.Values;
    /// <summary>
    /// 
    /// </summary>
    public int Count => exceptionStatusCodes.Count;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool ContainsKey(Type key)
    {
        return exceptionStatusCodes.ContainsKey(key);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator<KeyValuePair<Type, IExceptionStatusCode>> GetEnumerator()
    {
        return exceptionStatusCodes.GetEnumerator();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="statusCode"></param>
    public bool TryAddExceptionStatusCode(Type type, IExceptionStatusCode statusCode)
    {
        return exceptionStatusCodes.TryAdd(type, statusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="statusCode"></param>
    public bool TryAddExceptionStatusCode<TException>(IExceptionStatusCode statusCode)
    {
        return TryAddExceptionStatusCode(typeof(TException), statusCode);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGetValue(Type key, [MaybeNullWhen(false)] out IExceptionStatusCode value)
    {
        return exceptionStatusCodes.TryGetValue(key, out value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
