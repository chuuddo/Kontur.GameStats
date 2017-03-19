using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Dtos;
using Kontur.GameStats.Server.Features.Matches;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Matches
{
    [TestFixture]
    [Category("Integration")]
    public class GetMatchResultsQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            var mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>()).CreateMapper();
            _connection = Path.GetTempFileName() + ".sdf";
            _context = new ApplicationDbContext(_connection);
            _context.Servers.Add(new Data.Server {Endpoint = "localhost"});
            _context.SaveChanges();
            _handler = new GetMatchResultsQuery.Handler(_context, mapper);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
            File.Delete(_connection);
        }

        private ApplicationDbContext _context;
        private string _connection;
        private GetMatchResultsQuery.Handler _handler;

        [Test]
        public async Task Should_return_match_result_when_match_exists()
        {
            var timestamp = DateTime.UtcNow;
            _context.Servers.First().Matches.Add(new Match {Timestamp = timestamp});
            _context.SaveChanges();

            var result = await _handler.Handle(new GetMatchResultsQuery("LOCALhost", timestamp));

            Assert.NotNull(result);
        }

        [Test]
        public async Task Should_return_null_when_match_does_not_exists()
        {
            var result = await _handler.Handle(new GetMatchResultsQuery("LOCALhost", DateTime.UtcNow));

            Assert.Null(result);
        }

        [Test]
        public async Task Should_return_null_when_server_does_not_exists()
        {
            var result = await _handler.Handle(new GetMatchResultsQuery(string.Empty, DateTime.UtcNow));

            Assert.Null(result);
        }
    }
}