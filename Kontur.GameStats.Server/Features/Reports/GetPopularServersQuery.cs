using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using MediatR;
using static System.Data.Entity.DbFunctions;

namespace Kontur.GameStats.Server.Features.Reports
{
    public class GetPopularServersQuery : IRequest<List<PopularServerDto>>
    {
        public GetPopularServersQuery(int count)
        {
            Count = count;
        }

        public int Count { get; }

        public class Handler : IAsyncRequestHandler<GetPopularServersQuery, List<PopularServerDto>>
        {
            private readonly ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<List<PopularServerDto>> Handle(GetPopularServersQuery query)
            {
                return await _dbContext.Matches
                    .GroupBy(x => x.Server.Id)
                    .Select(g => new
                    {
                        Id = g.Key,
                        Avg = g.Count() / (double) (DiffDays(g.Min(m => m.Timestamp), _dbContext.Matches.Max(m => m.Timestamp)) + 1)
                    })
                    .Select(x => new PopularServerDto
                    {
                        Endpoint = _dbContext.Servers.FirstOrDefault(s => s.Id == x.Id).Endpoint,
                        Name = _dbContext.Servers.FirstOrDefault(s => s.Id == x.Id).Name,
                        AverageMatchesPerDay = x.Avg
                    })
                    .OrderByDescending(x => x.AverageMatchesPerDay)
                    .Take(query.Count)
                    .ToListAsync();
            }
        }
    }
}