using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Features.Reports;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Reports
{
    [TestFixture]
    [Category("Integration")]
    public class GetBestPlayersQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _connection = Path.GetTempFileName() + ".sdf";
            _context = new ApplicationDbContext(_connection);
            new TestDbInitializer().InitializeDatabase(_context);
            _handler = new GetBestPlayersQuery.Handler(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
            File.Delete(_connection);
        }

        private ApplicationDbContext _context;
        private string _connection;
        private GetBestPlayersQuery.Handler _handler;

        [Test]
        public async Task Should_calculate_kill_to_death_ratio()
        {
            var result = await _handler.Handle(new GetBestPlayersQuery(1));

            Assert.NotNull(result);
            Assert.AreEqual(3, result.First().KillToDeathRatio);
        }

        [Test]
        public async Task Should_return_only_players_with_10_matches_and_deaths_more_than_zero()
        {
            var result = await _handler.Handle(new GetBestPlayersQuery(5));

            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.Name == "Player 6"));
            Assert.IsTrue(result.Any(p => p.Name == "Player 7"));
        }
    }
}