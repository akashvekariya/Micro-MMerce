using FluentValidation;
using MediatR;

/// <summary>
/// This is the part of the MediatR pipeline.
/// This class will be used to validate the each incoming request.
/// If the request is not valid it will throw an exception,
/// If the request is valid it will be passed to the next step in the pipeline.
/// In this case it will be the handler;
/// The handler will be called only if the request is valid.
/// Reference: https://medium.com/swlh/mediator-pattern-in-asp-net-core-applications-pipelines-ec0926e71bc8
/// </summary>
namespace Ordering.Application.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // this line will collect all the validators form the assembly
    // in this case it will be the validators from the Ordering.Application.Features.Orders.Commands section
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_validators
            .Select(v => v.ValidateAsync(context, cancellationToken))
        );
        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            throw new Exceptions.ValidationException(failures);
        }

        return await next();
    }
}
