using MassTransit.Courier;
using System.Threading.Tasks;
using Transfer.Application.Interfaces;
using Transfer.Application.Orchestrators.RoutingSlip.Activities;

namespace Transfer.Application.Orchestrators.Activities.ProcessOutflow
{
    public class ProcessOutflowActivity :
        BaseActivity<ProcessOutflowArguments>
    {
        public ProcessOutflowActivity(IBankAccountClient accountClient)
            : base(accountClient)
        { }

        /// <summary>
        /// Выполняем списание денежных средств. 
        /// </summary>
        public override async Task<ExecutionResult> Execute(ExecuteContext<ProcessOutflowArguments> context)
        {
            await _accountClient.ProcessWithdrawal(
                context.Arguments.AccountId,
                context.Arguments.Sum,
                context.Arguments.CorrelationId);

            return context.Completed(new
            {
                context.Arguments.AccountId,
                context.Arguments.Sum,
                context.Arguments.CorrelationId
            });
        }

        /// <summary>
        /// Компенсация списания денежных средств. 
        /// </summary>
        public override async Task<CompensationResult> Compensate(CompensateContext<ActivityLog> context)
        {
            await _accountClient.ProcessDeposite(
                context.Log.AccountId,
                context.Log.Sum,
                context.Log.CorrelationId);

            return context.Compensated();
        }
    }
}
