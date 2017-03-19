using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using Kontur.GameStats.Server.Infrastructure;
using MediatR;
using static System.StringComparison;

namespace Kontur.GameStats.Server.Features.Matches
{
    public class PutMatchResultsCommand : IRequest
    {
        public PutMatchResultsCommand(string endpoint, DateTime timestamp, MatchResultsDto matchResults)
        {
            Endpoint = endpoint;
            Timestamp = timestamp;
            MatchResults = matchResults;
        }

        public string Endpoint { get; }
        public DateTime Timestamp { get; }
        public MatchResultsDto MatchResults { get; }

        public class Handler : IAsyncRequestHandler<PutMatchResultsCommand>
        {
            private readonly ApplicationDbContext _dbContext;

            public Handler(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task Handle(PutMatchResultsCommand command)
            {
                var server = await _dbContext.Servers.SingleOrDefaultAsync(x => x.Endpoint == command.Endpoint);

                if (server == null)
                    throw new ValidationException("Endpoint", "Server not found");

                if (server.Matches.Any(x => x.Timestamp == command.Timestamp))
                    throw new ValidationException("Timestamp", "Match already exists on this server.");

                var match = new Match
                {
                    Timestamp = command.Timestamp,
                    GameMode = await _dbContext.GameModes.SingleOrDefaultAsync(x => x.Name == command.MatchResults.GameMode) ??
                               new GameMode {Name = command.MatchResults.GameMode},
                    Map = await _dbContext.Maps.SingleOrDefaultAsync(x => x.Name == command.MatchResults.Map) ??
                          new Map {Name = command.MatchResults.Map},
                    FragLimit = command.MatchResults.FragLimit,
                    TimeLimit = command.MatchResults.TimeLimit,
                    TimeElapsed = command.MatchResults.TimeElapsed,
                    Server = server
                };

                var count = command.MatchResults.Scoreboard.Count;
                var playerNames = command.MatchResults.Scoreboard.Select(x => x.Name).ToList();
                var playersInDb = await _dbContext.Players.Where(x => playerNames.Contains(x.Name)).ToListAsync();
                var scores = command.MatchResults.Scoreboard.Select(
                    (score, i) =>
                        new Score
                        {
                            Deaths = score.Deaths,
                            Frags = score.Frags,
                            Kills = score.Kills,
                            ScoreboardPercent = count == 1 ? 100 : (count - (i + 1d)) / (count - 1d) * 100,
                            Player =
                                playersInDb.SingleOrDefault(x => x.Name.Equals(score.Name, OrdinalIgnoreCase)) ??
                                new Player {Name = score.Name},
                            Match = match
                        });

                _dbContext.Matches.Add(match);
                _dbContext.Scores.AddRange(scores);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}