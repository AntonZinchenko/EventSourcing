using MassTransit.Courier;
using System.Threading.Tasks;
using Transfer.Application.Interfaces;

namespace Transfer.Application.Orchestrators.RoutingSlip.Activities
{
    public class ProcessInflowActivity :
        BaseActivity<ProcessInflowArguments>
    {
        public ProcessInflowActivity(IBankAccountClient accountClient)
            : base(accountClient) 
        { }

        /// <summary>
        /// Выполняем зачисление денежных средств. 
        /// </summary>
        public override async Task<ExecutionResult> Execute(ExecuteContext<ProcessInflowArguments> context)
        {
            await _accountClient.ProcessDeposite(
                context.Arguments.AccountId,
                context.Arguments.Sum,
                context.Arguments.CorrelationId);

            return context.Completed();
        }
    }
}
