using Automatonymous;
using Bank.Orchestrators.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Bank.Orchestrators.Transfer.RoutingSlip
{
    public class TransferStateMachine : MassTransitStateMachine<TransferState>
    {
        public TransferStateMachine(ILogger<TransferState> logger)
        {
            InstanceState(x => x.CurrentState);

            ConfigureCorrelationIds();

            Initially(
                When(ExecuteTransferEvent)
                    .Then(x => logger.LogInformation($"Банковский перевод со счета {x.Instance.SourceAccountId} на счет {x.Instance.TargetAccountId}."))
                    .Then(InitStateMachine)
                    .TransitionTo(ReadyToStart)
                    .Then(x => x.Publish(new StartProcessing(x.Instance.CorrelationId))));

            During(ReadyToStart,
                When(ProcessStartedEvent)
                    .TransitionTo(PendingWithdrawalFinalization)
                    .Publish(context =>
                        new TransferSubmitted(
                            context.Instance.SourceAccountId,
                            context.Instance.TargetAccountId,
                            context.Instance.Sum,
                            context.Instance.CorrelationId)));

            During(PendingWithdrawalFinalization,
                 When(WithdrawalCompletedEvent)
                    .Then(x => logger.LogInformation($"Выполнено успешное списание денежных средств в размере {x.Instance.Sum} со счета {x.Instance.SourceAccountId}."))
                    .TransitionTo(PendingDepositeFinalization));

            During(PendingDepositeFinalization,
                  When(DepositeCompletedEvent)
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
        private void InitStateMachine(BehaviorContext<TransferState, ISumTransferStarted> ctx)
        {
            ctx.Instance.SourceAccountId = ctx.Data.SourceAccountId;
            ctx.Instance.TargetAccountId = ctx.Data.TargetAccountId;
            ctx.Instance.Sum = ctx.Data.Sum;
        }

        /// <summary>
        /// Уведомить систему мониторинга.
        /// </summary>
        private Task NotifyMonitoringService(BehaviorContext<TransferState, ActionFaulted> context)
            => Task.Run(() => { /*..*/ });

        private void ConfigureCorrelationIds()
        {
            Event(() => ProcessStartedEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => ExecuteTransferEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => WithdrawalCompletedEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => DepositeCompletedEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
            Event(() => OperationFaultedEvent, x => x.CorrelateById(x => x.Message.CorrelationId));
        }

        public State ReadyToStart { get; private set; }
        public State PendingWithdrawalFinalization { get; private set; }
        public State PendingDepositeFinalization { get; private set; }
        public State Completed { get; private set; }
        public State Faulted { get; private set; }

        public Event<ISumTransferStarted> ExecuteTransferEvent { get; set; }
        public Event<StartProcessing> ProcessStartedEvent { get; set; }
        public Event<IWithdrawalPerformed> WithdrawalCompletedEvent { get; set; }
        public Event<IDepositePerformed> DepositeCompletedEvent { get; set; }
        public Event<ActionFaulted> OperationFaultedEvent { get; set; }
    }
}
