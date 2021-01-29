using MassTransit.Courier;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Bank.Orchestrators.Transfer.RoutingSlip.Activities
{
    public abstract class BaseActivity<TArguments> :
        IActivity<TArguments, ActivityLog> where TArguments : class
    {
        protected readonly IServiceProvider _serviceProvider;

        public BaseActivity(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual Task<CompensationResult> Compensate(CompensateContext<ActivityLog> context)
            => Task.FromResult(context.Compensated());

        public abstract Task<ExecutionResult> Execute(ExecuteContext<TArguments> context);

        protected async Task SendCommand<TResponse>(IRequest<TResponse> command)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }
    }
}
