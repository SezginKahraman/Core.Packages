using Core.Application.Pipelines.Caching;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Serilog;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Core.Application.Pipelines.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ILoggableRequest
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly LoggerServiceBase _loggerServiceBase;

        public LoggingBehavior(IHttpContextAccessor contextAccessor, LoggerServiceBase loggerServiceBase)
        {
            _contextAccessor = contextAccessor;
            _loggerServiceBase = loggerServiceBase;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            List<LogParameter> logParameters = new()
            {
                new LogParameter { Type = request.GetType().Name, Value = request }
            };

            LogDetail logDetail = new()
            {
                MethodName = next.Method.Name,
                Parameters = logParameters,
                User = _contextAccessor.HttpContext.User.Identity?.Name ?? "NoName",
            };

            _loggerServiceBase.Info(JsonSerializer.Serialize(logDetail));

            return await next();
        } 
    }
}
