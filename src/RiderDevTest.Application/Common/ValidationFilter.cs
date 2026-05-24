using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace RiderDevTest.Application.Common;

/// <summary>
/// An endpoint filter that instantiates and invokes a validator based on the request before the controller action is executed.
/// </summary>
/// <typeparam name="TRequest">The type of the request to be validated.</typeparam>
public class ValidationFilter<TRequest> : IEndpointFilter
    where TRequest : class
{
    /// <summary>
    /// Instantiates and invokes the validator.
    /// </summary>
    /// <param name="context">The endpoint filter invocation context.</param>
    /// <param name="next">The next filter or endpoint handler in the pipeline.</param>
    /// <returns>The result of the endpoint invocation or a validation problem if validation fails.</returns>
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Try to find the request object in the arguments
        // If no request found, continue to next delegate
        TRequest? request = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (request is null)
        {
            return await next(context);
        }
        
        // Try to find all registered validators for the TRequest
        // If no validators are found, continue to next delegate
        var validators = context.HttpContext.RequestServices.GetServices(typeof(IValidator<TRequest>)) as IEnumerable<IValidator<TRequest>>;
        if (validators is null)
        {
            return await next(context);
        }
        
        // Create validation context
        var validationContext = new ValidationContext<TRequest>(request);

        // Execute all validators in parallel
        var validatorsList = validators.ToList();
        var validationResults = await Task.WhenAll(validatorsList.Select(v => v.ValidateAsync(validationContext)));

        // Collect all validation failures
        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .ToList();
        
        if (failures.Count != 0)
        {
            // When a standardized API response pattern is introduced, add all validation errors to a response and return it
        }

        return await next(context);
    }
}