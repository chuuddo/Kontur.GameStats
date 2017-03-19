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
    public class GetServerInfoQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
            _connection = Path.GetTempFileName() + ".sdf";
            _context = new ApplicationDbContext(_connection);
            _handler = new GetServerInfoQuery.Handler(_context, mapper);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
            File.Delete(_connection);
        }

        private ApplicationDbContext _context;
        private string _connection;
        private GetServerInfoQuery.Handler _handler;

        [Test]
        public async Task Should_return_null_when_server_does_not_exists()
        {
            var result = await _handler.Handle(new GetServerInfoQuery(string.Empty));

            Assert.Null(result);
        }

        [Test]
        public async Task Should_return_server_info_when_server_exists()
        {
            var server =
                new Data.Server
                {
                    Endpoint = "localhost",
                    Name = string.Empty,
                    GameModes = new List<GameMode> {new GameMode {Name = "DM"}, new GameMode {Name = "TDM"}}
                };
            _context.Servers.Add(server);
            _context.SaveChanges();

            var result = await _handler.Handle(new GetServerInfoQuery("LOCALhost"));

            Assert.NotNull(result);
            Assert.AreEqual(string.Empty, result.Name);
            Assert.AreEqual(2, result.GameModes.Count);
        }
    }
}