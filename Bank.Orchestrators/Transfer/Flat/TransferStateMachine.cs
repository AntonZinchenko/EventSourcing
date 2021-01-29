using Automatonymous;
using Bank.Application.Accounts.Commands;
using Bank.Orchestrators.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SeedWorks;
using System;
using System.Threading.Tasks;

namespace Bank.Orchestrators.Transfer.Flat
{
    public class TransferStateMachine : MassTransitStateMachine<TransferState>
    {
        private readonly IServiceProvider _serviceProvider;

        public TransferStateMachine(ILogger<TransferState> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InstanceState(x => x.CurrentState);

            ConfigureCorrelationIds();

            Initially(
                When(TransferStartedEvent)
                    .Then(x => logger.LogInformation($"Старт перевода со счета {x.Instance.SourceAccountId} на счет {x.Instance.TargetAccountId}."))
                    .Then(PrepareForProcessing)
                    .TransitionTo(OutFlowWaiting)
                    .ThenAsync(ProcessOutFlowOperation));

            During(OutFlowWaiting,
                 When(OperationFaultedEvent)
                    .Then(x => logger.LogInformation($"Отмена списания денежных средств со счета {x.Instance.SourceAccountId}."))
                    .ThenAsync(RollbackWithdrawal),
                 When(OutflowPerformedEvent)
                    .Then(x => logger.LogInformation($"Выполнено успешное списание денежных средств в размере {x.Instance.Sum} со счета {x.Instance.SourceAccountId}."))
                    .TransitionTo(InFlowWaiting)
                    .ThenAsync(ProcessInFlowOperation));

            During(InFlowWaiting,
                  When(InflowPerformedEvent)
                     .Then(x => logger.LogInformation($"Выполнено успешное зачисление денежных средств в размере {x.Instance.Sum} на счет {x.Instance.TargetAccountId}."))
                     .Then(x => x.Instance.Comment = "Transaction completed successfully.")
                     .TransitionTo(Completed)
                     .Finalize());

            DuringAny(
                  When(OperationFaultedEvent)
                     .Then(x => logger.LogInformation($"Перевод денежных средств со счета {x.Instance.SourceAccountId} на счет {x.Instance.TargetAccountId} закончился неудачей! Причина: [{DateTime.Now}] {x.Data.Reason}."))
                     .ThenAsync(NotifyMonitoringService)
                     .Then(x => x.Instance.Comment = x.Data.Reason)
                     .TransitionTo(Faulted));
        }

        /// <summary>
        /// Инициация состояния конечного автомата.
        /// </summary>
        private void PrepareForProcessing(BehaviorContext<TransferState, ISumTransferStarted> ctx)
        {
            ctx.Instance.SourceAccountId = ctx.Data.SourceAccountId;
            ctx.Instance.TargetAccountId = ctx.Data.TargetAccountId;
            ctx.Instance.Sum = ctx.Data.Sum;
        }

        /// <summary>
        /// Выполнить списание денежных средств. 
        /// </summary>
        private async Task ProcessOutFlowOperation(BehaviorContext<TransferState, ISumTransferStarted> context)
            => await new PerformWithdrawalCommand(context.Instance.SourceAccountId, context.Instance.Sum, context.Instance.CorrelationId)
                .PipeTo(async command => await SendCommand(command));

        /// <summary>
        /// Выполнить зачисление денежных средств.
        /// </summary>
        private async Task ProcessInFlowOperation(BehaviorContext<TransferState, IWithdrawalPerformed> context)
            => await new PerformDepositeCommand(context.Instance.TargetAccountId, context.Instance.Sum, context.Instance.CorrelationId)
                .PipeTo(async command => await SendCommand(command));

        /// <summary>
        /// Откатить списание денежных средств. 
        /// </summary>
        private async Task RollbackWithdrawal(BehaviorContext<TransferState, ActionFaulted> context)
            => await new PerformDepositeCommand(context.Instance.SourceAccountId, context.Instance.Sum, context.Instance.CorrelationId)
                .PipeTo(async command => await SendCommand(command));

        /// <summary>
        /// Уведомить систему мониторинга.
        /// </summary>
        private Task NotifyMonitoringService(BehaviorContext<TransferState, ActionFaulted> context)
            => Task.Run(() => { });

        private async Task SendCommand<TResponse>(IRequest<TResponse> command)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }

        private void ConfigureCorrelationIds()
        {
            Event(() => TransferStartedEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => OutflowPerformedEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => InflowPerformedEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => OperationFaultedEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
        }

        public State Started { get; private set; }
        public State OutFlowWaiting { get; set; }
        public State InFlowWaiting { get; set; }
        public State Completed { get; private set; }
        public State Faulted { get; private set; }

        public Event<ISumTransferStarted> TransferStartedEvent { get; set; }
        public Event<IWithdrawalPerformed> OutflowPerformedEvent { get; set; }
        public Event<IDepositePerformed> InflowPerformedEvent { get; set; }
        public Event<ActionFaulted> OperationFaultedEvent { get; set; }
    }
}
