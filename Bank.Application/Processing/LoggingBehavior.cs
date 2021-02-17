using System;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Contracts.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SeedWorks;
using SeedWorks.Core.Events;

namespace BankAccount.Application.Processing
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly IExecutionContextAccessor _executionContextAccessor;
        private readonly IEventBus _eventBus;

        public LoggingBehavior(
            ILogger<LoggingBehavior<TRequest, TResponse>> logger,
            IEventBus eventBus,
            IExecutionContextAccessor executionContextAccessor)
        {
            _logger = logger;
            _eventBus = eventBus;
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
                _logger.LogError(e, $"[{GetCorrelationId(request)}] {e.Message}");

                if (request is ISagaRequest action)
                {
                    await _eventBus.Publish(new ActionFaulted(e.Message, action.CorrelationId, request.GetType().Name));
                }

                throw;
            }
        }

        private Guid GetCorrelationId(TRequest request)
            => request switch
            {
                ISagaRequest source => source.CorrelationId,
                _ => _executionContextAccessor.CorrelationId,
            };
    }
}