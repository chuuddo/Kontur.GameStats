using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using Kontur.GameStats.Server.Features.Servers;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Servers
{
    [TestFixture]
    [Category("Integration")]
    public class GetAllServersQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
            _connection = Path.GetTempFileName() + ".sdf";
            _context = new ApplicationDbContext(_connection);
            _handler = new GetAllServersQuery.Handler(_context, mapper);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
            File.Delete(_connection);
        }

        private ApplicationDbContext _context;
        private string _connection;
        private GetAllServersQuery.Handler _handler;

        [Test]
        public async Task Should_return_empty_list_when_servers_does_not_exists()
        {
            var result = await _handler.Handle(new GetAllServersQuery());

            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async Task Should_return_list_of_servers_when_any_server_exists()
        {
            var server =
                new Data.Server
                {
                    Endpoint = string.Empty,
                    Name = string.Empty,
                    GameModes = new List<GameMode> {new GameMode {Name = "DM"}, new GameMode {Name = "TDM"}}
                };
            _context.Servers.Add(server);
            _context.SaveChanges();

            var result = await _handler.Handle(new GetAllServersQuery());

            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count);
        }
    }
}