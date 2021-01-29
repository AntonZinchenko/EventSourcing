using Bank.Orchestrators.Contracts;
using Bank.Orchestrators.Transfer.RoutingSlip.Activities;
using Bank.Orchestrators.Transfer.RoutingSlip.Activities.ProcessOutflow;
using MassTransit;
using MassTransit.Courier;
using System;
using System.Threading.Tasks;

namespace Bank.Orchestrators.Transfer.RoutingSlip
{
    public class ProcessTransferJobConsumer : IConsumer<TransferSubmitted>
    {
        public async Task Consume(ConsumeContext<TransferSubmitted> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            builder.AddActivity(
                typeof(ProcessOutflowActivity).Name,
                new Uri("queue:process-outflow_execute"),
                new
                {
                    AccountId = context.Message.SourceAccountId,
                    context.Message.Sum,
                    context.Message.CorrelationId
                });

         builder.AddActivity(
             typeof(ProcessInflowActivity).Name,
             new Uri("queue:process-inflow_execute"),
             new
             {
                 AccountId = context.Message.TargetAccountId,
                 context.Message.Sum,
                 context.Message.CorrelationId
             });

            await context.Execute(builder.Build());
        }
    }
}
