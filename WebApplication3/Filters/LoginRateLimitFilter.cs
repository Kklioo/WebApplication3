using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebApplication3.Filters
{
    public class LoginRateLimitFilter : ActionFilterAttribute
    {
        private readonly int _maxAttempts;
        private readonly TimeSpan _period;
        private readonly IMemoryCache _cache;

        public LoginRateLimitFilter(int maxAttempts, TimeSpan period)
        {
            _maxAttempts = maxAttempts;
            _period = period;
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress;

            var cacheKey = $"{ipAddress}_LoginAttempts";
            var attempts = _cache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpiration = DateTime.UtcNow.Add(_period);
                return 0;
            });

            if (attempts >= _maxAttempts)
            {
                context.Result = new ContentResult
                {
                    Content = "Too many login attempts. Try again later.",
                    StatusCode = 429
                };
                return;
            }

            _cache.Set(cacheKey, attempts + 1);

            base.OnActionExecuting(context);
        }
    }
}