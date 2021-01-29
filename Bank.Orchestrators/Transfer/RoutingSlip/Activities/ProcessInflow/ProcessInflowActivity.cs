using Bank.Application.Accounts.Commands;
using MassTransit.Courier;
using SeedWorks;
using System;
using System.Threading.Tasks;

namespace Bank.Orchestrators.Transfer.RoutingSlip.Activities
{
    public class ProcessInflowActivity :
        BaseActivity<ProcessInflowArguments>
    {
        public ProcessInflowActivity(IServiceProvider serviceProvider)
            : base(serviceProvider) 
        { }

        public override async Task<ExecutionResult> Execute(ExecuteContext<ProcessInflowArguments> context)
        {
            // Выполняем зачисление
            await new PerformDepositeCommand(context.Arguments.AccountId, context.Arguments.Sum, context.Arguments.CorrelationId)
                .PipeTo(async command => await SendCommand(command));

            return context.Completed();
        }
    }
}
