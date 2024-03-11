using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching
{
    public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICacheRemoverRequest
    {
        private readonly CacheSettings _cacheSettings;
        private readonly IDistributedCache _distributedCache;

        public CacheRemovingBehavior(IDistributedCache distributedCache, IConfiguration configuration)
        {
            _cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? throw new InvalidOperationException();
            _distributedCache = distributedCache;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request.BypassCache)
                return await next();

            TResponse response = await next();
            if (!string.IsNullOrWhiteSpace(request.CacheKey))
            { 
                await _distributedCache.RemoveAsync(request.CacheKey, cancellationToken);
            }

            return response;
        }
    }
}
