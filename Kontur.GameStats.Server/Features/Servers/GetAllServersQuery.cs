using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using MediatR;

namespace Kontur.GameStats.Server.Features.Servers
{
    public class GetAllServersQuery : IRequest<List<ServerDto>>
    {
        public class Handler : IAsyncRequestHandler<GetAllServersQuery, List<ServerDto>>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<List<ServerDto>> Handle(GetAllServersQuery query)
            {
                return await _dbContext.Servers.ProjectTo<ServerDto>(_mapper.ConfigurationProvider).ToListAsync();
            }
        }
    }
}