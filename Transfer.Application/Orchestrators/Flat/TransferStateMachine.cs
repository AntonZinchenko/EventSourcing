using AutoMapper;
using Automatonymous;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Transfer.Application.Interfaces;
using Transfer.Contracts.Events;

namespace Transfer.Application.Orchestrators.Flat
{
    public class TransferStateMachine : MassTransitStateMachine<TransferState>
    {
        private readonly IBankAccountClient _accountClient;

        public TransferStateMachine(
            ILogger<TransferState> logger,
            IMapper mapper,
            IBankAccountClient accountClient)
        {
            _accountClient = accountClient;

            InstanceState(x => x.CurrentState);

            ConfigureCorrelationIds();

            Initially(
                When(ExecuteTransferEvent)
                    .Then(x =>
                    {
                        logger.LogInformation($"Банковский перевод со счета {x.Instance.SourceAccountId} на счет {x.Instance.TargetAccountId}.");

                        InitStateMachine(x);
                    })
                    .TransitionTo(ReadyToStart)
                    .Publish(x => mapper.Map<StartProcessing>(x.Instance)));

            During(ReadyToStart,
                When(ProcessStartedEvent)
                    .TransitionTo(PendingWithdrawalFinalization)
                    .ThenAsync(ProcessOutFlowOperation));

            During(PendingWithdrawalFinalization,
                When(OperationFaultedEvent)
                    .Then(x => logger.LogInformation($"Отмена списания денежных средств со счета {x.Instance.SourceAccountId}."))
                    .ThenAsync(RollbackWithdrawal),
                When(WithdrawalCompletedEvent)
                    .Then(x =>
                    {
                        logger.LogInformation($"Выполнено успешное списание денежных средств в размере {x.Instance.Sum} со счета {x.Instance.SourceAccountId}.");

                        x.Instance.SourceAccountVersion = x.Data.AccountVersion;
                    })
                    .TransitionTo(PendingDepositeFinalization)
                    .ThenAsync(ProcessInFlowOperation));

            During(PendingDepositeFinalization,
                When(DepositeCompletedEvent)
                     .Then(x =>
                     {
                         logger.LogInformation($"Выполнено успешное зачисление денежных средств в размере {x.Instance.Sum} на счет {x.Instance.TargetAccountId}.");

                         x.Instance.Comment = "Transaction completed successfully.";
                         x.Instance.TargetAccountVersion = x.Data.AccountVersion;
                     })
                     .TransitionTo(Completed)
                     .Finalize());

            DuringAny(
                When(OperationFaultedEvent)
                     .Then(x =>
                     {
                         logger.LogInformation($"Перевод денежных средств со счета {x.Instance.SourceAccountId} на счет {x.Instance.TargetAccountId} закончился неудачей! Причина: [{DateTime.Now}] {x.Data.Reason}.");

                         x.Instance.Comment = x.Data.Reason;
                     })
                    .ThenAsync(NotifyMonitoringService)
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
        /// Выполнить списание денежных средств. 
        /// </summary>
        private async Task ProcessOutFlowOperation(BehaviorContext<TransferState, StartProcessing> context)
            => await _accountClient.ProcessWithdrawal(
                context.Instance.SourceAccountId,
                context.Instance.Sum,
                context.Instance.CorrelationId);

        /// <summary>
        /// Выполнить зачисление денежных средств.
        /// </summary>
        private async Task ProcessInFlowOperation(BehaviorContext<TransferState, IWithdrawalPerformed> context)
            => await _accountClient.ProcessDeposite(
                context.Instance.TargetAccountId,
                context.Instance.Sum,
                context.Instance.CorrelationId);

        /// <summary>
        /// Откатить списание денежных средств. 
        /// </summary>
        private async Task RollbackWithdrawal(BehaviorContext<TransferState, ActionFaulted> context)
            => await _accountClient.ProcessDeposite(
                context.Instance.SourceAccountId,
                context.Instance.Sum,
                context.Instance.CorrelationId);

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
