using AutoMapper;
using Transfer.Contracts.Events;
using Transfer.Contracts.Types;
using Transfer.Storage;

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
                .ForMember(dto => dto.Id, conf => conf.MapFrom(ol => ol.CorrelationId));
        }
    }
}
