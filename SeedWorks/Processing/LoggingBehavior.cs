using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SeedWorks.Core.Events;

namespace SeedWorks.Processing
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        protected readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        protected readonly IExecutionContextAccessor _executionContextAccessor;

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
                _logger.LogInformation($"[{GetCorrelationId(request)}] Request {typeof(TRequest).Name}: {JsonConvert.SerializeObject(request)}");
                var response = await next();
                _logger.LogInformation($"[{GetCorrelationId(request)}] Response {typeof(TResponse).Name}: {JsonConvert.SerializeObject(response)}");

                return response;
            }
            catch (Exception e)
            {
                await HandleException(request, e);

                throw;
            }
        }

        protected virtual Task HandleException(TRequest request, Exception e)
            => Task.CompletedTask
                .Do(_ => _logger.LogError(e, $"[{GetCorrelationId(request)}] {e.Message}"));

        private Guid GetCorrelationId(TRequest request)
            => request switch
            {
                ISagaRequest source => source.CorrelationId,
                _ => _executionContextAccessor.CorrelationId,
            };
    }
}