using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SeedWorks.Processing
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;

        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger,
            IExecutionContextAccessor executionContextAccessor)
        {
            _logger = logger;
            _executionContextAccessor = executionContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                _logger.LogInformation($"[{_executionContextAccessor.CorrelationId}] Request {typeof(TRequest).Name}: {JsonConvert.SerializeObject(request)}");
                var response = await next();
                _logger.LogInformation($"[{_executionContextAccessor.CorrelationId}] Response {typeof(TResponse).Name}: {JsonConvert.SerializeObject(response)}");

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"[{_executionContextAccessor.CorrelationId}] {e.Message}");
                throw;
            }
        }
    }
}