using System.Linq;
using AutoMapper;
using Kontur.GameStats.Server.Data;

namespace Kontur.GameStats.Server.Dtos
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            ApplicationDbContext context = null;

            CreateMap<Data.Server, ServerInfoDto>()
                .ForMember(x => x.GameModes, x => x.MapFrom(src => src.GameModes.GroupBy(g => g.Name).Select(g => g.Key)));

            CreateMap<Data.Server, ServerDto>()
                .ForMember(x => x.Info, x => x.MapFrom(src => src));

            CreateMap<Match, MatchResultsDto>()
                .ForMember(x => x.GameMode, x => x.MapFrom(src => src.GameMode.Name))
                .ForMember(x => x.Map, x => x.MapFrom(src => src.Map.Name))
                .ForMember(x => x.Scoreboard, x => x.MapFrom(src => src.ScoreBoard.OrderByDescending(s => s.ScoreboardPercent)));

            CreateMap<Match, MatchDto>()
                .ForMember(x => x.Endpoint, x => x.MapFrom(src => src.Server.Endpoint))
                .ForMember(x => x.Results, x => x.MapFrom(src => src));

            CreateMap<Score, ScoreDto>()
                .ForMember(x => x.Name, x => x.MapFrom(src => context.Players.FirstOrDefault(p => p.Id == src.PlayerId).Name));
        }
    }
}