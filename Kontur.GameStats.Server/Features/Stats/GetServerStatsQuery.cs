using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using MediatR;
using static System.Data.Entity.DbFunctions;

namespace Kontur.GameStats.Server.Features.Stats
{
    public class GetServerStatsQuery : IRequest<ServerStatsDto>
    {
        public GetServerStatsQuery(string endpoint)
        {
            Endpoint = endpoint;
        }

        public string Endpoint { get; }

        public class Handler : IAsyncRequestHandler<GetServerStatsQuery, ServerStatsDto>
        {
            private readonly ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<ServerStatsDto> Handle(GetServerStatsQuery query)
            {
                return await _dbContext.Servers
                    .Where(s => s.Endpoint == query.Endpoint)
                    .Select(x => x.Matches)
                    .Select(x => new ServerStatsDto
                    {
                        TotalMatchesPlayed = x.Count,
                        MaximumPopulation = x.OrderByDescending(m => m.ScoreBoard.Count).Select(m => m.ScoreBoard.Count).FirstOrDefault(),
                        AveragePopulation = x.SelectMany(m => m.ScoreBoard).Count() / (double)x.Count,
                        MaximumMatchesPerDay = x.GroupBy(m => TruncateTime(m.Timestamp)).Max(g => g.Count()),
                        AverageMatchesPerDay =
                            x.Count / (double) (DiffDays(x.Min(m => m.Timestamp), _dbContext.Matches.Max(m => m.Timestamp)) + 1),
                        Top5GameModes =
                            x.GroupBy(m => m.GameMode).OrderByDescending(g => g.Count()).Take(5).Select(g => g.Key.Name).ToList(),
                        Top5Maps =
                            x.GroupBy(m => m.Map).OrderByDescending(g => g.Count()).Take(5).Select(g => g.Key.Name).ToList()
                    }).SingleOrDefaultAsync();
            }
        }
    }
}