﻿using Transfer.Application.Orchestrators.RoutingSlip.Activities;
using MassTransit;
using MassTransit.Courier;
using System;
using System.Threading.Tasks;
using Transfer.Application.Orchestrators.Activities.ProcessOutflow;
using Transfer.Contracts.Events;

namespace Transfer.Application.Orchestrators.RoutingSlip
{
    public class ProcessTransferJobConsumer : IConsumer<ExecuteActivities>
    {
        public async Task Consume(ConsumeContext<ExecuteActivities> context)
        {
            // Инициация RoutingSlip
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            // Операция списания денежных средств
            builder.AddActivity(
                nameof(ProcessOutflowActivity),
                new Uri("queue:process-outflow_execute"),
                new
                {
                    AccountId = context.Message.SourceAccountId,
                    context.Message.Sum,
                    context.Message.CorrelationId
                });
            
            // Операция зачисления денежных средств
            builder.AddActivity(
                nameof(ProcessInflowActivity),
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
