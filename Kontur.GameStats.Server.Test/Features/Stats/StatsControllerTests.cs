using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using FakeItEasy;
using Kontur.GameStats.Server.Dtos;
using Kontur.GameStats.Server.Features.Stats;
using MediatR;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Stats
{
    [TestFixture]
    [Category("Unit")]
    public class StatsControllerTests
    {
        [SetUp]
        public void Setup()
        {
            _fakeMediator = A.Fake<IMediator>();
            _statsController = new StatsController(_fakeMediator);
        }

        private StatsController _statsController;
        private IMediator _fakeMediator;

        [Test]
        public async Task GetPlayerStats_should_return_not_found_when_server_does_not_exists()
        {
            A.CallTo(() => _fakeMediator.Send(A<GetPlayerStatsQuery>._, A<CancellationToken>._)).Returns(null as PlayerStatsDto);

            var result = await _statsController.GetPlayerStats(string.Empty);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetPlayerStats_should_return_player_stats_when_player_exists()
        {
            var playerStatsDto = new PlayerStatsDto {TotalMatchesPlayed = 50};
            A.CallTo(() => _fakeMediator.Send(A<GetPlayerStatsQuery>._, A<CancellationToken>._)).Returns(playerStatsDto);

            var result = await _statsController.GetPlayerStats(string.Empty) as OkNegotiatedContentResult<PlayerStatsDto>;

            Assert.NotNull(result);
            Assert.AreEqual(50, result.Content.TotalMatchesPlayed);
        }

        [Test]
        public async Task GetServerStats_should_return_not_found_when_server_does_not_exists()
        {
            A.CallTo(() => _fakeMediator.Send(A<GetServerStatsQuery>._, A<CancellationToken>._)).Returns(null as ServerStatsDto);

            var result = await _statsController.GetServerStats(string.Empty);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetServerStats_should_return_server_stats_when_server_exists()
        {
            var serverStatsDto = new ServerStatsDto {TotalMatchesPlayed = 100};
            A.CallTo(() => _fakeMediator.Send(A<GetServerStatsQuery>._, A<CancellationToken>._)).Returns(serverStatsDto);

            var result = await _statsController.GetServerStats(string.Empty) as OkNegotiatedContentResult<ServerStatsDto>;

            Assert.NotNull(result);
            Assert.AreEqual(100, result.Content.TotalMatchesPlayed);
        }
    }
}