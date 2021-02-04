using AutoMapper;
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
        public ProcessOutflowActivity(IServiceProvider serviceProvider, IMapper mapper)
            : base(serviceProvider, mapper)
        { }

        /// <summary>
        /// Выполняем списание денежных средств. 
        /// </summary>
        public override async Task<ExecutionResult> Execute(ExecuteContext<ProcessOutflowArguments> context)
        {
            await _mapper.Map<PerformWithdrawalCommand>(context.Arguments)
                .PipeTo(async command => await SendCommand(command));

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
            await _mapper.Map<PerformDepositeCommand>(context.Log)
               .PipeTo(async command => await SendCommand(command));

            return context.Compensated();
        }
    }
}
