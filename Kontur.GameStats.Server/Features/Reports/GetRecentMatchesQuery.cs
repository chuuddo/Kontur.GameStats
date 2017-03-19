using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using MediatR;

namespace Kontur.GameStats.Server.Features.Reports
{
    public class GetRecentMatchesQuery : IRequest<List<MatchDto>>
    {
        public GetRecentMatchesQuery(int count)
        {
            Count = count;
        }

        public int Count { get; }

        public class Handler : IAsyncRequestHandler<GetRecentMatchesQuery, List<MatchDto>>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<List<MatchDto>> Handle(GetRecentMatchesQuery query)
            {
                return await _dbContext.Matches
                    .OrderByDescending(x => x.Timestamp)
                    .Take(query.Count)
                    .ProjectTo<MatchDto>(_mapper.ConfigurationProvider, new { context = _dbContext })
                    .ToListAsync();
            }
        }
    }
}