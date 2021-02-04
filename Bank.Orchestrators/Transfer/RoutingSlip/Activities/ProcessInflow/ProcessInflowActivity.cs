using AutoMapper;
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
        public ProcessInflowActivity(IServiceProvider serviceProvider, IMapper mapper)
            : base(serviceProvider, mapper) 
        { }

        /// <summary>
        /// Выполняем зачисление денежных средств. 
        /// </summary>
        public override async Task<ExecutionResult> Execute(ExecuteContext<ProcessInflowArguments> context)
        {
            await _mapper.Map<PerformDepositeCommand>(context.Arguments)
                .PipeTo(async command => await SendCommand(command));

            return context.Completed();
        }
    }
}
