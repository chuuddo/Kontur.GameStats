using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Features.Stats;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Stats
{
    [TestFixture]
    [Category("Integration")]
    public class GetPlayerStatsQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _connection = Path.GetTempFileName() + ".sdf";
            _context = new ApplicationDbContext(_connection);
            new TestDbInitializer().InitializeDatabase(_context);
            _handler = new GetPlayerStatsQuery.Handler(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
            File.Delete(_connection);
        }

        private static IEnumerable TestCases()
        {
            yield return new TestCaseData("TotalMatchesPlayed", 3);
            yield return new TestCaseData("TotalMatchesWon", 1);
            yield return new TestCaseData("FavoriteServer", "localhost-1337");
            yield return new TestCaseData("UniqueServers", 2);
            yield return new TestCaseData("FavoriteGameMode", "DM");
            yield return new TestCaseData("AverageScoreboardPercent", 100.0 / 3);
            yield return new TestCaseData("MaximumMatchesPerDay", 1);
            yield return new TestCaseData("AverageMatchesPerDay", 3.0 / 4);
            yield return new TestCaseData("LastMatchPlayed", DateTime.Parse("2017-03-17T15:00:00Z").ToUniversalTime());
            yield return new TestCaseData("KillToDeathRatio", 24.0 / 46);
        }

        private ApplicationDbContext _context;
        private string _connection;
        private GetPlayerStatsQuery.Handler _handler;

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public async Task Should_be_equal(string property, object value)
        {
            var result = await _handler.Handle(new GetPlayerStatsQuery("Player 2"));

            Assert.AreEqual(value, result.GetType().GetProperty(property).GetValue(result, null));
        }

        [Test]
        public async Task Should_calculate_kill_to_death_ratio_when_sum_of_deaths_equals_zero()
        {
            var result = await _handler.Handle(new GetPlayerStatsQuery("Player 4"));

            Assert.AreEqual(40, result.KillToDeathRatio);
        }

        [Test]
        public async Task Should_return_null_when_player_does_not_exists()
        {
            var result = await _handler.Handle(new GetPlayerStatsQuery(string.Empty));

            Assert.Null(result);
        }

        [Test]
        public async Task Should_return_player_stats_when_player_exists()
        {
            var result = await _handler.Handle(new GetPlayerStatsQuery("Player 2"));

            Assert.NotNull(result);
        }
    }
}