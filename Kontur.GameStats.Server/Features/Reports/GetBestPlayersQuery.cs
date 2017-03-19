using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using MediatR;

namespace Kontur.GameStats.Server.Features.Reports
{
    public class GetBestPlayersQuery : IRequest<List<BestPlayerDto>>
    {
        public GetBestPlayersQuery(int count)
        {
            Count = count;
        }

        public int Count { get; }

        public class Handler : IAsyncRequestHandler<GetBestPlayersQuery, List<BestPlayerDto>>
        {
            private readonly ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<List<BestPlayerDto>> Handle(GetBestPlayersQuery query)
            {
                return await _dbContext.Scores
                    .GroupBy(x => x.PlayerId)
                    .Where(g => g.Count() >= 10)
                    .Select(g => new
                    {
                        Id = g.Key,
                        TotalKills = g.Sum(s => s.Kills),
                        TotalDeaths = g.Sum(s => s.Deaths)
                    })
                    .Where(x => x.TotalDeaths > 0)
                    .Select(x => new BestPlayerDto
                    {
                        Name = _dbContext.Players.FirstOrDefault(s => s.Id == x.Id).Name,
                        KillToDeathRatio = x.TotalKills / (double) x.TotalDeaths
                    })
                    .OrderByDescending(x => x.KillToDeathRatio)
                    .Take(query.Count)
                    .ToListAsync();
            }
        }
    }
}