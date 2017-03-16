using System.Linq;
using AutoMapper;
using Kontur.GameStats.Server.Dtos;

namespace Kontur.GameStats.Server.Features.Servers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Data.Server, ServerInfoDto>()
                .ForMember(x => x.GameModes, x => x.MapFrom(src => src.GameModes.GroupBy(g => g.Name).Select(g => g.Key)));

            CreateMap<Data.Server, ServerDto>()
                .ForMember(x => x.Info, x => x.MapFrom(src => src));
        }
    }
}