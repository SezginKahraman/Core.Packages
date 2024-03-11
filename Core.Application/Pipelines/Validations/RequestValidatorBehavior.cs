using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Exceptions.Types;
using FluentValidation;
using MediatR;
using ValidationException = FluentValidation.ValidationException;

namespace Core.Application.Pipelines.Validations
{
    public class RequestValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> // piplines are middle for cqrs pattern. 
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidatorBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationContext<object> context = new(request);

            IEnumerable<ValidationExceptionModel> errors = _validators
                .Select(validator => validator.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(failure => failure!=null)
                .GroupBy(keySelector: p=> p.PropertyName, resultSelector: (propertyName, errors) => 
                    new ValidationExceptionModel(){Property = propertyName, Errors = errors.Select(e => e.ErrorMessage)}).ToList();

            if (errors.Any())
                throw new CrossCuttingConcerns.Exceptions.Types.ValidationException(errors);

            TResponse response = await next();

            return response;
        }
    }
}
