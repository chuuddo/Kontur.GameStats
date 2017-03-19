using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using Kontur.GameStats.Server.Features.Matches;
using Kontur.GameStats.Server.Infrastructure;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Matches
{
    [TestFixture]
    [Category("Integration")]
    public class PutMatchResultsCommandHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _connection = Path.GetTempFileName() + ".sdf";
            _context = new ApplicationDbContext(_connection);
            _context.Servers.Add(new Data.Server {Endpoint = "localhost"});
            _context.SaveChanges();
            _handler = new PutMatchResultsCommand.Handler(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
            File.Delete(_connection);
        }

        private ApplicationDbContext _context;
        private string _connection;
        private PutMatchResultsCommand.Handler _handler;

        [Test]
        public async Task Should_add_new_gamemode_when_does_not_exists()
        {
            var matchResults = new MatchResultsDto
            {
                GameMode = "DM",
                Scoreboard = new List<ScoreDto>()
            };

            await _handler.Handle(new PutMatchResultsCommand("LOCALhost", DateTime.UtcNow, matchResults));

            Assert.AreEqual(1, _context.GameModes.Count());
        }

        [Test]
        public async Task Should_add_new_map_when_does_not_exists()
        {
            var matchResults = new MatchResultsDto
            {
                Map = "King of the kill",
                Scoreboard = new List<ScoreDto>()
            };

            await _handler.Handle(new PutMatchResultsCommand("LOCALhost", DateTime.UtcNow, matchResults));

            Assert.AreEqual(1, _context.Maps.Count());
        }

        [Test]
        public async Task Should_add_new_match()
        {
            var matchResults = new MatchResultsDto
            {
                Map = "Hello World",
                GameMode = "DM",
                FragLimit = 10,
                TimeLimit = 10,
                TimeElapsed = 1.2368,
                Scoreboard =
                    new List<ScoreDto>
                    {
                        new ScoreDto {Name = "Player 1", Kills = 1, Frags = 1, Deaths = 0},
                        new ScoreDto {Name = "Player 2", Kills = 0, Frags = 0, Deaths = 1}
                    }
            };

            await _handler.Handle(new PutMatchResultsCommand("LOCALhost", DateTime.UtcNow, matchResults));

            Assert.AreEqual(1, _context.Matches.Count());
            Assert.AreEqual(1, _context.GameModes.Count());
            Assert.AreEqual(1, _context.Maps.Count());
            Assert.AreEqual(2, _context.Players.Count());
            Assert.AreEqual(1.2368, _context.Matches.First().TimeElapsed);
        }

        [Test]
        public async Task Should_add_new_players_when_does_not_exist()
        {
            var matchResults = new MatchResultsDto
            {
                Scoreboard = new List<ScoreDto> {new ScoreDto {Name = "TAZ"}}
            };

            await _handler.Handle(new PutMatchResultsCommand("LOCALhost", DateTime.UtcNow, matchResults));

            Assert.AreEqual(1, _context.Players.Count());
            Assert.AreEqual("TAZ", _context.Players.First().Name);
        }

        [Test]
        public async Task Should_calculate_scoreboard_percent_for_each_score()
        {
            var matchResults = new MatchResultsDto
            {
                Scoreboard = new List<ScoreDto>
                {
                    new ScoreDto {Name = "Player 1"},
                    new ScoreDto {Name = "Player 2"},
                    new ScoreDto {Name = "Player 3"}
                }
            };

            await _handler.Handle(new PutMatchResultsCommand("LOCALhost", DateTime.UtcNow, matchResults));

            Assert.AreEqual(new List<double> {100, 50, 0}, _context.Scores.Select(x => x.ScoreboardPercent));
        }

        [Test]
        public void Should_throw_exception_when_match_already_exists()
        {
            var timestamp = DateTime.UtcNow;
            _context.Servers.First().Matches.Add(new Match {Timestamp = timestamp});
            _context.SaveChanges();
            var putMatchResultsCommand = new PutMatchResultsCommand("LOCALhost", timestamp, new MatchResultsDto());

            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(putMatchResultsCommand));
            Assert.AreEqual("Timestamp", exception.Property);
        }

        [Test]
        public void Should_throw_exception_when_server_does_not_exist()
        {
            var putMatchResultsCommand = new PutMatchResultsCommand(string.Empty, DateTime.UtcNow, new MatchResultsDto());

            var exception = Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(putMatchResultsCommand));
            Assert.AreEqual("Endpoint", exception.Property);
        }

        [Test]
        public async Task Should_use_existing_gamemode()
        {
            _context.GameModes.Add(new GameMode {Name = "DM"});
            _context.SaveChanges();
            var matchResults = new MatchResultsDto
            {
                GameMode = "dm",
                Scoreboard = new List<ScoreDto>()
            };

            await _handler.Handle(new PutMatchResultsCommand("LOCALhost", DateTime.UtcNow, matchResults));

            Assert.AreEqual(1, _context.GameModes.Count());
            Assert.AreEqual("DM", _context.Matches.First().GameMode.Name);
        }

        [Test]
        public async Task Should_use_existing_map()
        {
            _context.Maps.Add(new Map {Name = "King of the kill"});
            _context.SaveChanges();
            var matchResults = new MatchResultsDto
            {
                Map = "King of THE kill",
                Scoreboard = new List<ScoreDto>()
            };

            await _handler.Handle(new PutMatchResultsCommand("LOCALhost", DateTime.UtcNow, matchResults));

            Assert.AreEqual(1, _context.Maps.Count());
            Assert.AreEqual("King of the kill", _context.Matches.First().Map.Name);
        }

        [Test]
        public async Task Should_use_existing_players()
        {
            _context.Players.Add(new Player {Name = "TAZ"});
            _context.SaveChanges();
            var matchResults = new MatchResultsDto
            {
                Scoreboard = new List<ScoreDto> {new ScoreDto {Name = "tAz"}}
            };

            await _handler.Handle(new PutMatchResultsCommand("LOCALhost", DateTime.UtcNow, matchResults));

            Assert.AreEqual(1, _context.Players.Count());
            Assert.AreEqual("TAZ", _context.Matches.First().ScoreBoard.First().Player.Name);
        }
    }
}