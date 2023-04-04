using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace RST.FluentValidation.Extensions;

/// <summary>
/// 
/// </summary>
public class FluentValidationRequestHandler<TRequest>
    : MediatR.Pipeline.IRequestPreProcessor<TRequest>
    where TRequest:notnull
{
    private readonly IValidator<TRequest>? validationContext;

    /// <summary>
    /// 
    /// </summary>
    public FluentValidationRequestHandler(IServiceProvider serviceProvider)
    {
        this.validationContext = serviceProvider.GetService<IValidator<TRequest>>();
    }

    /// <inheritdoc cref="MediatR.Pipeline.IRequestPreProcessor{TRequest}"/>
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if(validationContext == null)
        {
            return;
        }

        var validationResults = await validationContext.ValidateAsync(request, cancellationToken);
        if(!validationResults.IsValid)
        {
            throw new ValidationException(validationResults.Errors);
        }
    }
}
