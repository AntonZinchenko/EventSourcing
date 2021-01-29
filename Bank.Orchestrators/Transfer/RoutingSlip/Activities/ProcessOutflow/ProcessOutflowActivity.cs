using Bank.Application.Accounts.Commands;
using MassTransit.Courier;
using SeedWorks;
using System;
using System.Threading.Tasks;

namespace Bank.Orchestrators.Transfer.RoutingSlip.Activities.ProcessOutflow
{
    public class ProcessOutflowActivity :
        BaseActivity<ProcessOutflowArguments>
    {
        public ProcessOutflowActivity(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override async Task<ExecutionResult> Execute(ExecuteContext<ProcessOutflowArguments> context)
        {
            // Выполняем списание
            await new PerformWithdrawalCommand(context.Arguments.AccountId, context.Arguments.Sum, context.Arguments.CorrelationId)
                .PipeTo(async command => await SendCommand(command));

            return context.Completed(new
            {
                context.Arguments.AccountId,
                context.Arguments.Sum,
                context.Arguments.CorrelationId
            });
        }

        
        public override async Task<CompensationResult> Compensate(CompensateContext<ActivityLog> context)
        {
            // Компенсация списания
            await new PerformDepositeCommand(context.Log.AccountId, context.Log.Sum, context.Log.CorrelationId)
               .PipeTo(async command => await SendCommand(command));

            return context.Compensated();
        }
    }
}
