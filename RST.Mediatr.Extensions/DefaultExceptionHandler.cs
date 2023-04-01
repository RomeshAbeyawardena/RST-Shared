using AutoMapper;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using RST.Defaults;
using RST.Mediatr.Extensions.Contracts;

namespace RST.Mediatr.Extensions;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <typeparam name="TException"></typeparam>
public class DefaultExceptionHandler<TRequest, TResponse, TException>
    : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : notnull
    where TException : Exception
{
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IExceptionHandlerExceptionStatusCodeFactory exceptionHandlerExceptionStatusCodeFactory;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mapper"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="exceptionHandlerExceptionStatusCodeFactory"></param>
    public DefaultExceptionHandler(IMapper mapper, 
        IHttpContextAccessor httpContextAccessor,
        IExceptionHandlerExceptionStatusCodeFactory exceptionHandlerExceptionStatusCodeFactory)
    {
        this.mapper = mapper;
        this.httpContextAccessor = httpContextAccessor;
        this.exceptionHandlerExceptionStatusCodeFactory = exceptionHandlerExceptionStatusCodeFactory;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="exception"></param>
    /// <param name="state"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        var exceptionType = typeof(TException);
        
        var @interface = typeof(TRequest).GetInterfaces().FirstOrDefault();

        Type? genericArgument;
        if (exceptionHandlerExceptionStatusCodeFactory.TryGetValue(exceptionType, out var exceptionStatusCode)
            && @interface != null
            && (genericArgument = @interface.GetGenericArguments().LastOrDefault()) != null
            && genericArgument.GetInterfaces().Any(a => a.IsInterface && a == typeof(RST.Contracts.IResult)))
        {
            var result = new DefaultResult
            {
                StatusCode = exceptionStatusCode.StatusCode,
                StatusMessage = string.IsNullOrEmpty(exceptionStatusCode.StatusMessage) 
                    ? exception.Message
                    : string.Format(exceptionStatusCode.StatusMessage, exception.Message)
            };

            state.SetHandled(mapper.Map<TResponse>(result));
            if (httpContextAccessor.HttpContext != null)
            {
                httpContextAccessor.HttpContext.Response.StatusCode = result.StatusCode.GetValueOrDefault(200);
            }
            return Task.CompletedTask;
        }
        else
            throw exception;
    }
}
