using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using MediatR;

namespace Kontur.GameStats.Server.Features.Servers
{
    public class GetServerInfoQuery : IRequest<ServerInfoDto>
    {
        public GetServerInfoQuery(string endpoint)
        {
            Endpoint = endpoint;
        }

        public string Endpoint { get; }

        public class Handler : IAsyncRequestHandler<GetServerInfoQuery, ServerInfoDto>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<ServerInfoDto> Handle(GetServerInfoQuery query)
            {
                return await _dbContext.Servers
                    .Where(x => x.Endpoint == query.Endpoint)
                    .ProjectTo<ServerInfoDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();
            }
        }
    }
}