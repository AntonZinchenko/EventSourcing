using System;
using System.Threading;
using System.Threading.Tasks;
using Transfer.Contracts.Events;
using Microsoft.Extensions.Logging;
using SeedWorks;
using SeedWorks.Core.Events;
using SeedWorks.Processing;

namespace BankAccount.Application.Processing
{
    public class SagaLoggingBehavior<TRequest, TResponse> : LoggingBehavior<TRequest, TResponse>
    {
        private readonly IEventBus _eventBus;

        public SagaLoggingBehavior(
            ILogger<SagaLoggingBehavior<TRequest, TResponse>> logger,
            IEventBus eventBus,
            IExecutionContextAccessor executionContextAccessor): base(logger, executionContextAccessor)
        {
            _eventBus = eventBus;
        }


        protected override async Task HandleException(TRequest request, Exception e)
        {
            await base.HandleException(request, e);

            if (request is ISagaRequest action)
            {
                await _eventBus.Publish(new ActionFaulted(e.Message, action.CorrelationId, request.GetType().Name));
            }
        }
    }
}