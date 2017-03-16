using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using Kontur.GameStats.Server.Features.Servers;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Servers
{
    [TestFixture]
    [Category("Integration")]
    public class PutServerInfoCommandHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _connection = Path.GetTempFileName() + ".sdf";
            _context = new ApplicationDbContext(_connection);
            _handler = new PutServerInfoCommand.Handler(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
            File.Delete(_connection);
        }

        private ApplicationDbContext _context;
        private string _connection;
        private PutServerInfoCommand.Handler _handler;

        [Test]
        public async Task Should_add_new_gamemodes_when_they_does_not_exists()
        {
            _context.GameModes.Add(new GameMode {Name = "DM"});
            _context.SaveChanges();
            var serverInfo = new ServerInfoDto {Name = string.Empty, GameModes = new List<string> {"dM", "TDM"}};

            await _handler.Handle(new PutServerInfoCommand(string.Empty, serverInfo));

            Assert.AreEqual(2, _context.GameModes.Count());
        }

        [Test]
        public async Task Should_add_new_server()
        {
            var serverInfo = new ServerInfoDto {Name = string.Empty, GameModes = new List<string>()};

            await _handler.Handle(new PutServerInfoCommand(string.Empty, serverInfo));

            Assert.AreEqual(1, _context.Servers.Count());
        }

        [Test]
        public async Task Should_update_existing_server()
        {
            var server =
                new Data.Server
                {
                    Endpoint = "localhost",
                    Name = string.Empty,
                    GameModes = new List<GameMode>()
                };
            _context.Servers.Add(server);
            _context.SaveChanges();
            var serverInfo = new ServerInfoDto {Name = "new name", GameModes = new List<string> {"DM"}};

            await _handler.Handle(new PutServerInfoCommand("LOCALhost", serverInfo));

            Assert.AreEqual(1, _context.Servers.Count());
            Assert.AreEqual("new name", _context.Servers.First().Name);
            Assert.AreEqual(1, _context.Servers.First().GameModes.Count);
        }
    }
}