using AutoMapper;
using Automatonymous;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Transfer.Storage;
using Transfer.Contracts.Events;

namespace Transfer.Application.Orchestrators.RoutingSlip
{
    public class TransferStateMachine : MassTransitStateMachine<TransferState>
    {
        public TransferStateMachine(ILogger<TransferState> logger, IMapper mapper)
        {
            InstanceState(x => x.CurrentState);

            ConfigureCorrelationIds();

            Initially(
                When(ExecuteTransferEvent)
                    .Then(x => logger.LogInformation($"Банковский перевод со счета {x.Instance.SourceAccountId} на счет {x.Instance.TargetAccountId}."))
                    .Then(InitStateMachine)
                    .TransitionTo(ReadyToStart)
                    .Publish(x => mapper.Map<StartProcessing>(x.Instance)));

            During(ReadyToStart,
                When(ProcessStartedEvent)
                    .TransitionTo(PendingWithdrawalFinalization)
                    .Publish(x => mapper.Map<ExecuteActivities>(x.Instance)));

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

        public State ReadyToStart { get; }
        public State PendingWithdrawalFinalization { get; }
        public State PendingDepositeFinalization { get; }
        public State Completed { get; }
        public State Faulted { get; }

        public Event<ISumTransferStarted> ExecuteTransferEvent { get; }
        public Event<StartProcessing> ProcessStartedEvent { get; }
        public Event<IWithdrawalPerformed> WithdrawalCompletedEvent { get; }
        public Event<IDepositePerformed> DepositeCompletedEvent { get; }
        public Event<ActionFaulted> OperationFaultedEvent { get; }
    }
}
