using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bank.Api
{
    internal class CorrelationMiddleware
    {
        internal const string CorrelationHeaderKey = "CorrelationId";
        private readonly RequestDelegate _next;

        public CorrelationMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request != null)
            {
                context.Request.Headers.Add(CorrelationHeaderKey, Guid.NewGuid().ToString());
            }

            await _next.Invoke(context);
        }
    }
}
