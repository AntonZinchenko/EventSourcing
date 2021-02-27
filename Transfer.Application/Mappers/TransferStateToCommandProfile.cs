using AutoMapper;
using Transfer.Application.Orchestrators;
using Transfer.Contracts.Events;
using Transfer.Contracts.Types;

namespace Transfer.Application.Mappers
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

            CreateMap<TransferState, TransferView>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(ol => ol.CorrelationId))
                .ForMember(dto => dto.State, conf => conf.MapFrom(ol => ol.CurrentState));
        }
    }
}
