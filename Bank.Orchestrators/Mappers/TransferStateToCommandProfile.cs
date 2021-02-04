using AutoMapper;
using Automatonymous;
using Bank.Application.Accounts.Commands;
using Bank.Orchestrators.Contracts;
using Bank.Orchestrators.Transfer;

namespace Bank.Orchestrators.Mappers
{
    public class TransferStateToCommandProfile: Profile
    {
        public TransferStateToCommandProfile()
        {
            CreateMap<TransferState, StartProcessing>()
                .ConstructUsing((src, ctx) => new StartProcessing(src.CorrelationId));

            CreateMap<TransferState, ExecuteActivities>()
                .ConstructUsing((src, ctx) => new ExecuteActivities(
                    src.SourceAccountId,
                    src.TargetAccountId,
                    src.Sum,
                    src.CorrelationId));

            CreateMap<BehaviorContext<TransferState, StartProcessing>, PerformWithdrawalCommand>()
                .ConstructUsing((src, ctx) => new PerformWithdrawalCommand(
                    src.Instance.SourceAccountId,
                    src.Instance.Sum,
                    src.Instance.CorrelationId));

            CreateMap<BehaviorContext<TransferState, IWithdrawalPerformed>, PerformDepositeCommand>()
                .ConstructUsing((src, ctx) => new PerformDepositeCommand(
                    src.Instance.TargetAccountId,
                    src.Instance.Sum,
                    src.Instance.CorrelationId));

            CreateMap<BehaviorContext<TransferState, ActionFaulted>, PerformDepositeCommand>()
                .ConstructUsing((src, ctx) => new PerformDepositeCommand(
                    src.Instance.SourceAccountId,
                    src.Instance.Sum,
                    src.Instance.CorrelationId));
        }
    }
}
