using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using Kontur.GameStats.Server.Data;
using Kontur.GameStats.Server.Features.Stats;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Stats
{
    [TestFixture]
    [Category("Integration")]
    public class GetServerStatsQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _connection = Path.GetTempFileName() + ".sdf";
            Database.SetInitializer(new TestDbInitializer());
            _context = new ApplicationDbContext(_connection);
            _handler = new GetServerStatsQuery.Handler(_context);
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
            yield return new TestCaseData("MaximumMatchesPerDay", 2);
            yield return new TestCaseData("AverageMatchesPerDay", 1);
            yield return new TestCaseData("MaximumPopulation", 3);
            yield return new TestCaseData("AveragePopulation", 8.0 / 3);
            yield return new TestCaseData("Top5GameModes", new List<string> {"DM", "TDM"})
                .SetName(@"Should_be_equal(""Top5GameModes"", [""DM"", ""TDM""])");
            yield return new TestCaseData("Top5Maps", new List<string> {"Map 2", "Map 1"})
                .SetName(@"Should_be_equal(""Top5Maps"", [""Map 2"", ""Map 1""])");
        }

        private ApplicationDbContext _context;
        private string _connection;
        private GetServerStatsQuery.Handler _handler;

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public async Task Should_be_equal(string property, object value)
        {
            var result = await _handler.Handle(new GetServerStatsQuery("Localhost-1337"));

            Assert.AreEqual(value, result.GetType().GetProperty(property).GetValue(result, null));
        }

        [Test]
        public async Task Should_return_null_when_server_does_not_exists()
        {
            var result = await _handler.Handle(new GetServerStatsQuery(string.Empty));

            Assert.Null(result);
        }

        [Test]
        public async Task Should_return_server_stats_when_server_exists()
        {
            var result = await _handler.Handle(new GetServerStatsQuery("Localhost-1337"));

            Assert.NotNull(result);
        }
    }
}