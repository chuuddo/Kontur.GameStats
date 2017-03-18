using System.Linq;
using AutoMapper;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;

namespace Kontur.GameStats.Server.Features.Matches
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Match, MatchResultsDto>()
                .ForMember(x => x.GameMode, x => x.MapFrom(src => src.GameMode.Name))
                .ForMember(x => x.Map, x => x.MapFrom(src => src.Map.Name))
                .ForMember(x => x.Scoreboard, x => x.MapFrom(src => src.ScoreBoard.OrderByDescending(s => s.ScoreboardPercent)));

            ApplicationDbContext context = null;
            CreateMap<Score, ScoreDto>()
                .ForMember(x => x.Name, x => x.MapFrom(src => context.Players.FirstOrDefault(p => p.Id == src.PlayerId).Name));
        }
    }
}