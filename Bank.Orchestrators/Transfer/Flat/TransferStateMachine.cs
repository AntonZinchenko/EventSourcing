using AutoMapper;
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
        private readonly IMapper _mapper;

        public TransferStateMachine(ILogger<TransferState> logger, IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;

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
                    .ThenAsync(ProcessOutFlowOperation));

            During(PendingWithdrawalFinalization,
                When(OperationFaultedEvent)
                    .Then(x => logger.LogInformation($"Отмена списания денежных средств со счета {x.Instance.SourceAccountId}."))
                    .ThenAsync(RollbackWithdrawal),
                When(WithdrawalCompletedEvent)
                    .Then(x => logger.LogInformation($"Выполнено успешное списание денежных средств в размере {x.Instance.Sum} со счета {x.Instance.SourceAccountId}."))
                    .TransitionTo(PendingDepositeFinalization)
                    .ThenAsync(ProcessInFlowOperation));

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
        /// Выполнить списание денежных средств. 
        /// </summary>
        private async Task ProcessOutFlowOperation(BehaviorContext<TransferState, StartProcessing> context)
            => await _mapper.Map<PerformWithdrawalCommand>(context)
                .PipeTo(async command => await SendCommand(command));

        /// <summary>
        /// Выполнить зачисление денежных средств.
        /// </summary>
        private async Task ProcessInFlowOperation(BehaviorContext<TransferState, IWithdrawalPerformed> context)
            => await _mapper.Map<PerformDepositeCommand>(context)
                .PipeTo(async command => await SendCommand(command));

        /// <summary>
        /// Откатить списание денежных средств. 
        /// </summary>
        private async Task RollbackWithdrawal(BehaviorContext<TransferState, ActionFaulted> context)
            => await _mapper.Map<PerformDepositeCommand>(context)
                .PipeTo(async command => await SendCommand(command));

        /// <summary>
        /// Уведомить систему мониторинга.
        /// </summary>
        private Task NotifyMonitoringService(BehaviorContext<TransferState, ActionFaulted> context)
            => Task.Run(() => { /*..*/ });

        private async Task SendCommand<TResponse>(IRequest<TResponse> command)
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }

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
