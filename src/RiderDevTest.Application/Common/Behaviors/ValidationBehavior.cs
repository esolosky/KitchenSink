using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace RiderDevTest.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidator<TRequest>? _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
        {
            return await next();
        }

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors;
            // .ConvertAll(error => Error.Validation(
            //     code: error.PropertyName,
            //     description: error.ErrorMessage));

        // ErrorOr<T> has implicit conversion from List<Error>, but we need to cast to TResponse.
        // Using (dynamic) triggers runtime implicit conversion. This is a known pattern with ErrorOr.
        return (dynamic)errors;
    }
}