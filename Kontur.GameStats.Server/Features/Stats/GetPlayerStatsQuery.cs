using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using MediatR;
using static System.Data.Entity.DbFunctions;

namespace Kontur.GameStats.Server.Features.Stats
{
    public class GetPlayerStatsQuery : IRequest<PlayerStatsDto>
    {
        public GetPlayerStatsQuery(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public class Handler : IAsyncRequestHandler<GetPlayerStatsQuery, PlayerStatsDto>
        {
            private readonly ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<PlayerStatsDto> Handle(GetPlayerStatsQuery query)
            {
                return await _dbContext.Players
                    .Where(s => s.Name == query.Name)
                    .Select(x => x.Scores)
                    .Select(x => new PlayerStatsDto
                    {
                        TotalMatchesPlayed = x.Count,
                        TotalMatchesWon = x.Count(s => Math.Abs(s.ScoreboardPercent - 100) < double.Epsilon),
                        FavoriteServer =
                            x.Select(s => s.Match)
                                .GroupBy(m => m.Server.Endpoint)
                                .OrderByDescending(g => g.Count())
                                .Select(g => g.Key)
                                .FirstOrDefault(),
                        UniqueServers = x.Select(s => s.Match).GroupBy(m => m.Server.Endpoint).Select(g => g.Key).Count(),
                        FavoriteGameMode =
                            x.Select(s => s.Match)
                                .GroupBy(m => m.GameMode.Name)
                                .OrderByDescending(g => g.Count())
                                .Select(g => g.Key)
                                .FirstOrDefault(),
                        AverageScoreboardPercent = x.Sum(s => s.ScoreboardPercent) / x.Count,
                        MaximumMatchesPerDay = x.Select(s => s.Match).GroupBy(m => TruncateTime(m.Timestamp)).Max(g => g.Count()),
                        AverageMatchesPerDay =
                            x.Count / (double) (DiffDays(x.Min(s => s.Match.Timestamp), _dbContext.Matches.Max(m => m.Timestamp)) + 1),
                        LastMatchPlayed = x.Max(s => s.Match.Timestamp),
                        KillToDeathRatio = x.Sum(s => s.Kills) / new[] {x.Sum(s => s.Deaths), 1.0}.Max()
                    }).SingleOrDefaultAsync();
            }
        }
    }
}