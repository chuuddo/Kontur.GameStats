using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Results;
using FakeItEasy;
using Kontur.GameStats.Server.Dtos;
using Kontur.GameStats.Server.Features.Reports;
using MediatR;
using NUnit.Framework;

namespace Kontur.GameStats.Server.Test.Features.Reports
{
    [TestFixture]
    [Category("Unit")]
    public class ReportsControllerTests
    {
        [SetUp]
        public void Setup()
        {
            _fakeMediator = A.Fake<IMediator>();
            _reportsController = new ReportsController(_fakeMediator);
        }

        private ReportsController _reportsController;
        private IMediator _fakeMediator;

        private static IEnumerable TestCases()
        {
            var cases = new List<TestCaseData>
            {
                new TestCaseData(0, 0),
                new TestCaseData(-1, 0),
                new TestCaseData(10, 10),
                new TestCaseData(60, 50)
            };
            foreach (var data in cases)
            {
                yield return
                    data.SetName($"Should_return_{data.OriginalArguments[1]}_objects_when_count_equals_{data.OriginalArguments[0]}");
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public async Task GetBestPlayers(int count, int expected)
        {
            var callTo =
                A.CallTo(() => _fakeMediator.Send(A<GetBestPlayersQuery>.That.Matches(x => x.Count == expected), A<CancellationToken>._));
            callTo.Returns(A.CollectionOfFake<BestPlayerDto>(expected).ToList());

            var result = await _reportsController.GetBestPlayers(count) as OkNegotiatedContentResult<List<BestPlayerDto>>;

            if (count > 0)
                callTo.MustHaveHappened();
            else
                callTo.MustNotHaveHappened();
            Assert.NotNull(result);
            Assert.AreEqual(expected, result.Content.Count);
        }

        [Test]
        public async Task GetBestPlayers_should_return_5_objects_when_count_not_specified()
        {
            var callTo =
                A.CallTo(() => _fakeMediator.Send(A<GetBestPlayersQuery>.That.Matches(x => x.Count == 5), A<CancellationToken>._));
            callTo.Returns(A.CollectionOfFake<BestPlayerDto>(5).ToList());

            var result = await _reportsController.GetBestPlayers() as OkNegotiatedContentResult<List<BestPlayerDto>>;

            callTo.MustHaveHappened();
            Assert.NotNull(result);
            Assert.AreEqual(5, result.Content.Count);
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public async Task GetPopularServers(int count, int expected)
        {
            var callTo =
                A.CallTo(() => _fakeMediator.Send(A<GetPopularServersQuery>.That.Matches(x => x.Count == expected), A<CancellationToken>._));
            callTo.Returns(A.CollectionOfFake<PopularServerDto>(expected).ToList());

            var result = await _reportsController.GetPopularServers(count) as OkNegotiatedContentResult<List<PopularServerDto>>;

            if (count > 0)
                callTo.MustHaveHappened();
            else
                callTo.MustNotHaveHappened();
            Assert.NotNull(result);
            Assert.AreEqual(expected, result.Content.Count);
        }

        [Test]
        public async Task GetPopularServers_should_return_5_objects_when_count_not_specified()
        {
            var callTo =
                A.CallTo(() => _fakeMediator.Send(A<GetPopularServersQuery>.That.Matches(x => x.Count == 5), A<CancellationToken>._));
            callTo.Returns(A.CollectionOfFake<PopularServerDto>(5).ToList());

            var result = await _reportsController.GetPopularServers() as OkNegotiatedContentResult<List<PopularServerDto>>;

            callTo.MustHaveHappened();
            Assert.NotNull(result);
            Assert.AreEqual(5, result.Content.Count);
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public async Task GetRecentMatches(int count, int expected)
        {
            var callTo =
                A.CallTo(() => _fakeMediator.Send(A<GetRecentMatchesQuery>.That.Matches(x => x.Count == expected), A<CancellationToken>._));
            callTo.Returns(A.CollectionOfFake<MatchDto>(expected).ToList());

            var result = await _reportsController.GetRecentMatches(count) as OkNegotiatedContentResult<List<MatchDto>>;

            if (count > 0)
                callTo.MustHaveHappened();
            else
                callTo.MustNotHaveHappened();
            Assert.NotNull(result);
            Assert.AreEqual(expected, result.Content.Count);
        }

        [Test]
        public async Task GetRecentMatches_should_return_5_objects_when_count_not_specified()
        {
            var callTo =
                A.CallTo(() => _fakeMediator.Send(A<GetRecentMatchesQuery>.That.Matches(x => x.Count == 5), A<CancellationToken>._));
            callTo.Returns(A.CollectionOfFake<MatchDto>(5).ToList());

            var result = await _reportsController.GetRecentMatches() as OkNegotiatedContentResult<List<MatchDto>>;

            callTo.MustHaveHappened();
            Assert.NotNull(result);
            Assert.AreEqual(5, result.Content.Count);
        }
    }
}