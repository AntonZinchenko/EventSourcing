using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SeedWorks
{
    public class CorrelationMiddleware
    {
        public const string CorrelationHeaderKey = "CorrelationId";
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var hasCorrelationToken = context.Request.Headers.Any(h => h.Key == CorrelationHeaderKey);
            if (!hasCorrelationToken)
            {
                context.Request.Headers.Add(CorrelationHeaderKey, Guid.NewGuid().ToString());
            }

            await _next.Invoke(context);
        }
    }
}
