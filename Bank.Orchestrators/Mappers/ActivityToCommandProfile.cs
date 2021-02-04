using AutoMapper;
using Bank.Application.Accounts.Commands;
using Bank.Orchestrators.Transfer.RoutingSlip.Activities;
using Bank.Orchestrators.Transfer.RoutingSlip.Activities.ProcessOutflow;

namespace Bank.Orchestrators.Mappers
{
    public class ActivityToCommandProfile: Profile
    {
        public ActivityToCommandProfile()
        {
            CreateMap<ProcessInflowArguments, PerformDepositeCommand>()
                .ConstructUsing((src, ctx) => new PerformDepositeCommand(src.AccountId, src.Sum, src.CorrelationId));

            CreateMap<ProcessOutflowArguments, PerformWithdrawalCommand>()
                .ConstructUsing((src, ctx) => new PerformWithdrawalCommand(src.AccountId, src.Sum, src.CorrelationId));

            CreateMap<ActivityLog, PerformDepositeCommand>()
                .ConstructUsing((src, ctx) => new PerformDepositeCommand(src.AccountId, src.Sum, src.CorrelationId));
        }
    }
}
