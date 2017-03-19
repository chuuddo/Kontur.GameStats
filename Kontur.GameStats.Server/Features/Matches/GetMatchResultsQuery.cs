using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using MediatR;

namespace Kontur.GameStats.Server.Features.Matches
{
    public class GetMatchResultsQuery : IRequest<MatchResultsDto>
    {
        public GetMatchResultsQuery(string endpoint, DateTime timestamp)
        {
            Endpoint = endpoint;
            Timestamp = timestamp;
        }

        public string Endpoint { get; }
        public DateTime Timestamp { get; }


        public class Handler : IAsyncRequestHandler<GetMatchResultsQuery, MatchResultsDto>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<MatchResultsDto> Handle(GetMatchResultsQuery query)
            {
                return await _dbContext.Matches
                    .Where(x => x.Server.Endpoint == query.Endpoint && x.Timestamp == query.Timestamp)
                    .ProjectTo<MatchResultsDto>(_mapper.ConfigurationProvider, new { context = _dbContext })
                    .SingleOrDefaultAsync();
            }
        }
    }
}