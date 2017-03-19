using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using MediatR;
using static System.StringComparison;

namespace Kontur.GameStats.Server.Features.Servers
{
    public class PutServerInfoCommand : IRequest
    {
        public PutServerInfoCommand(string endpoint, ServerInfoDto serverInfo)
        {
            Endpoint = endpoint;
            ServerInfo = serverInfo;
        }

        public string Endpoint { get; }
        public ServerInfoDto ServerInfo { get; }

        public class Handler : IAsyncRequestHandler<PutServerInfoCommand>
        {
            private readonly ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task Handle(PutServerInfoCommand command)
            {
                var server = await _dbContext.Servers.SingleOrDefaultAsync(x => x.Endpoint == command.Endpoint) ??
                                 new Data.Server {Endpoint = command.Endpoint};
                server.Name = command.ServerInfo.Name;
                server.GameModes.Clear();
                var gameModes = await _dbContext.GameModes.ToListAsync();
                foreach (var name in command.ServerInfo.GameModes)
                {
                    var gameMode = gameModes.SingleOrDefault(x => x.Name.Equals(name, OrdinalIgnoreCase)) ?? new GameMode {Name = name};
                    server.GameModes.Add(gameMode);
                }
                _dbContext.Entry(server).State = server.Id == 0 ? EntityState.Added : EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}