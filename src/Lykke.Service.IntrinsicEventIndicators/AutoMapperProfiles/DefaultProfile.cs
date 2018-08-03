using AutoMapper;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain;
using Lykke.Service.IntrinsicEventIndicators.Core.Domain.Model;
using Lykke.Service.IntrinsicEventIndicators.Models;

namespace Lykke.Service.IntrinsicEventIndicators.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<IntrinsicEventIndicatorsColumnPost, IntrinsicEventIndicatorsColumn>()
                .ForMember(x => x.ColumnId, opt => opt.Ignore());
            CreateMap<IIntrinsicEventIndicatorsColumn, IntrinsicEventIndicatorsColumnDto>();

            CreateMap<IntrinsicEventIndicatorsRowPost, IntrinsicEventIndicatorsRow>()
                .ForMember(x => x.RowId, opt => opt.Ignore());
            CreateMap<IntrinsicEventIndicatorsRowEdit, IntrinsicEventIndicatorsRow>()
                .ForMember(x => x.Exchange, opt => opt.Ignore())
                .ForMember(x => x.AssetPair, opt => opt.Ignore());

            CreateMap<IIntrinsicEventIndicatorsRow, IntrinsicEventIndicatorsRowDto>()
                .ForMember(x => x.AssetPair, opt => opt.MapFrom(src => !string.IsNullOrWhiteSpace(src.PairName) ? src.PairName : src.AssetPair));

            CreateMap<Core.Domain.Model.IntrinsicEventIndicators, IntrinsicEventIndicatorsDto>();

            CreateMap<IRunnerState, RunnerStateDto>();
        }
    }
}
