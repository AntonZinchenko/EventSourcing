using MassTransit.Courier;
using System.Threading.Tasks;
using Transfer.Application.Interfaces;

namespace Transfer.Application.Orchestrators.RoutingSlip.Activities
{
    public abstract class BaseActivity<TArguments> :
        IActivity<TArguments, ActivityLog> where TArguments : class
    {
        protected readonly IBankAccountClient _accountClient;

        public BaseActivity(IBankAccountClient accountClient)
        {
            _accountClient = accountClient;
        }

        public virtual Task<CompensationResult> Compensate(CompensateContext<ActivityLog> context)
            => Task.FromResult(context.Compensated());

        public abstract Task<ExecutionResult> Execute(ExecuteContext<TArguments> context);
    }
}
