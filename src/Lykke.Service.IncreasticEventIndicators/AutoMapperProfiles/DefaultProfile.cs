using AutoMapper;
using Lykke.Service.IncreasticEventIndicators.Core.Domain.Model;
using Lykke.Service.IncreasticEventIndicators.Models;

namespace Lykke.Service.IncreasticEventIndicators.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<IntrinsicEventIndicatorsColumnPost, IntrinsicEventIndicatorsColumn>()
                .ForMember(x => x.ColumnId, opt => opt.Ignore());
            CreateMap<IIntrinsicEventIndicatorsColumn, IntrinsicEventIndicatorsColumnDto>();
            CreateMap<IntrinsicEventIndicators, IntrinsicEventIndicatorsDto>();
        }
    }
}
