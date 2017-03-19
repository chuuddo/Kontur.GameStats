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
    public class GetPopularServersQueryHandlerTests
    {
        [SetUp]
        public void Setup()
        {
            _connection = Path.GetTempFileName() + ".sdf";
            _context = new ApplicationDbContext(_connection);
            new TestDbInitializer().InitializeDatabase(_context);
            _handler = new GetPopularServersQuery.Handler(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Dispose();
            File.Delete(_connection);
        }

        private ApplicationDbContext _context;
        private string _connection;
        private GetPopularServersQuery.Handler _handler;

        [Test]
        public async Task Should_calculate_average_matches_per_day()
        {
            var result = await _handler.Handle(new GetPopularServersQuery(1));

            Assert.NotNull(result);
            Assert.AreEqual(12, result.First().AverageMatchesPerDay);
        }
    }
}