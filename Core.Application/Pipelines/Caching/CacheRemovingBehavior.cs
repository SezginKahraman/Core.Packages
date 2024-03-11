using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Core.Application.Pipelines.Caching
{
    public class CacheRemovingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICacheRemoverRequest
    {
        private readonly CacheSettings _cacheSettings;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger _logger;

        public CacheRemovingBehavior(IDistributedCache distributedCache, IConfiguration configuration, ILogger logger)
        {
            _cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? throw new InvalidOperationException();
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request.BypassCache)
                return await next();

            TResponse response = await next();
            if (!string.IsNullOrWhiteSpace(request.CacheGroupKey))
            {
                byte[]? cachedGroup = await _distributedCache.GetAsync(request.CacheGroupKey, cancellationToken);
                if (cachedGroup != null)
                {
                    HashSet<string> keysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cachedGroup));
                    foreach (string key in keysInGroup)
                    {
                        await _distributedCache.RemoveAsync(key, cancellationToken);
                        _logger.LogInformation($"Removed Cache -> {key}");
                    }

                    await _distributedCache.RemoveAsync(request.CacheGroupKey, cancellationToken);
                    _logger.LogInformation($"Removed Cache group -> {request.CacheGroupKey}");

                    await _distributedCache.RemoveAsync(key:$"{request.CacheGroupKey}SlidingExpiration", cancellationToken);
                    _logger.LogInformation($"Removed Cache group -> {request.CacheGroupKey}SlidingExpiration");
                }
                await _distributedCache.RemoveAsync(request.CacheKey, cancellationToken);
            }

            if (request.CacheKey != null)
            {
                await _distributedCache.RemoveAsync(request.CacheKey, cancellationToken);
            }

            return response;
        }
    }
}
